using System;
using bkfc.Areas.Identity.Data;
using bkfc.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(bkfc.Areas.Identity.IdentityHostingStartup))]
namespace bkfc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<bkfcUserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("bkfcUserContextConnection")));

                services.AddDefaultIdentity<bkfcUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<bkfcUserContext>();
            });
        }
    }
}