using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Dii_OrderingSvc.Fake.Data;
using Dii_OrderingSvc.Fake.Features.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Dii_OrderingSvc.Fake
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string _policyName = "CorsPolicy";

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

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));
            ;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dii_OrderingSvc.Fake", Version = "v1" });
            });
            services.AddDbContext<OrderingSvcContext>(options =>
                    options.UseInMemoryDatabase(nameof(OrderingSvcContext)));

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OrderingSvcContext context)
        {
           // if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dii_OrderingSvc.Fake v1"));
            }

            app.UseRouting();
            app.UseCors(_policyName);

            app.UseAuthentication();
            app.UseAuthorization();

            DataSeeding.SeedData(context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
