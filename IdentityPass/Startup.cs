using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityPass.Models;
using IdentityPass.Services;
using IdentityPass.Services.Descriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CustomClaims = IdentityPass.Models.ClaimTypes;
namespace IdentityPass
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "CookieAuth";
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.OnlyAdmin, policy =>
                {
                    policy.RequireClaim(CustomClaims.IsAdmin, "True");
                });
                options.AddPolicy(Policies.OnlyAdminHR, policy =>
                {
                    policy
                    .RequireClaim(CustomClaims.IsAdmin, "True")
                    .RequireClaim(CustomClaims.IsHR, "True");
                });
                options.AddPolicy(Policies.OnlyHR, policy =>
                {
                    policy.RequireClaim(CustomClaims.IsHR, "True");
                });
            });
            services.AddScoped(typeof(IIdentityService), typeof(IdentityService));
            services.AddScoped(typeof(IFileService), typeof(FileService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
