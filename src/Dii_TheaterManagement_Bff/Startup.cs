using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr.Client;
using Dii_TheaterManagement_Bff.Clients;
using Dii_TheaterManagement_Bff.Features.FakeUsers;
using Dii_TheaterManagement_Bff.Features.SyntheticBehavior;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Dii_TheaterManagement_Bff
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";
        public static string OrderingHttpClientBaseAddress = string.Empty;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Allow a JWT bearer token but do not validate it.
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        // fool the validation logic
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JwtSecurityToken(token);
                            return jwt;
                        },
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = false,
                        RequireExpirationTime = false,
                        RequireSignedTokens = false
                    };
                });
            services.AddHealthChecks();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dii_TheaterManagement_Bff",
                    Version = "v1",
                    Description = "Welcome, tester. This is a SwaggerUI page that's available in Development and on the stage site. " +
                      "Some of the endpoints require authentication; Bearer tokens my be found under the /fake-users endpoint."
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                       "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                  });
            });

            services.AddHttpContextAccessor();
            services.AddScoped<UserInfoAccessor>();
            services.AddScoped<FakeUserFactory>();

            services.AddHttpContextAccessor();
            services.AddApplicationServices();
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //  if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dii_TheaterManagement_Bff v1"));
            }

            app.UseRouting();
            app.UseCors(_policyName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc");
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            if (Startup.OrderingHttpClientBaseAddress == string.Empty)
            {
                services.AddSingleton(typeof(OrderingSvcClient), serviceProvider =>
                {
                    var httpClient = DaprClient.CreateInvokeHttpClient("diiorderingsvc");
                    return new OrderingSvcClient(httpClient);
                });
                services.AddSingleton(typeof(MovieCatalogSvcClient), serviceProvider =>
                {
                    var httpClient = DaprClient.CreateInvokeHttpClient("diimoviecatalogsvc");
                    return new MovieCatalogSvcClient(httpClient);
                });
            }
            else
            {
                services.AddHttpClient(nameof(OrderingSvcClient), (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri(Startup.OrderingHttpClientBaseAddress);
                })
            .AddTypedClient<OrderingSvcClient>();
            }
            return services;
        }
    }
}
