using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolovniAutomobiliZavrsniRad.Data;

[assembly: HostingStartup(typeof(PolovniAutomobiliZavrsniRad.Areas.Identity.IdentityHostingStartup))]
namespace PolovniAutomobiliZavrsniRad.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}