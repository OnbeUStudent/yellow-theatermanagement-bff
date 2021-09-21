using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dii_TheaterManagement_Bff.Acceptance.Tests.Steps
{
    public class CustomWebApplicationFactory<TStartup>
      : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override IHostBuilder CreateHostBuilder()
        {

            Startup.OrderingHttpClientBaseAddress = "TBD";
            var builder = Host.CreateDefaultBuilder()
                               .ConfigureWebHostDefaults(webBuilder =>
                               {
                                   webBuilder.UseStartup<TStartup>();
                               });

            return builder;
        }


    }
}
