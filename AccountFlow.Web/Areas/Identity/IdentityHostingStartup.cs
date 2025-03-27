using System;
[assembly: HostingStartup(typeof(AccountFlow.Web.Areas.Identity.IdentityHostingStartup))]
namespace AccountFlow.Web.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/Account/Login");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/Account/Register");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", "/Account/AccessDenied");

                });
        });
    }

}
