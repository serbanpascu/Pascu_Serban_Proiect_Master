using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pascu_Serban_Proiect.Data;
using Microsoft.EntityFrameworkCore;
using Pascu_Serban_Proiect.Hubs;
using Microsoft.AspNetCore.Identity;


namespace Pascu_Serban_Proiect
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
            services.AddControllersWithViews();
            services.AddDbContext<ToyShopContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSignalR();
            services.AddRazorPages();

            services.Configure<IdentityOptions>(options => {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); 
                options.Lockout.MaxFailedAccessAttempts = 3; 
                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("OnlySales", policy =>
                {
                    policy.RequireClaim("Departament", "Sales");
                });
            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("SalesManager", policy =>
                {
                    policy.RequireRole("Manager");
                    policy.RequireClaim("Department", "Sales");
                });
            });

            services.ConfigureApplicationCookie(opts =>
            {
                opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<CustomersChat>("/customersChat");
                endpoints.MapRazorPages();
            });
        }
    }
}
