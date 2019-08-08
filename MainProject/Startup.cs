﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MainProject
{
    public class Startup
    {
        IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEntityFrameworkNpgsql().AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_configuration["DbConnection"]));//ders:62
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders(); ;

            services.Configure<IdentityOptions>(options => { //ders 60
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.AddSession();
            services.AddDistributedMemoryCache();//session nerede tutulacak : uygulmaa sunucusu hafızası
            services.ConfigureApplicationCookie(options => {//ders 61
                options.LoginPath = "/Security/LogIn";
                options.LogoutPath = "/Security/LogOut";
                options.AccessDeniedPath = "/Security/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".aspnet.security.cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,//uygulma dışından istekte bulunma
                    SecurePolicy = CookieSecurePolicy.SameAsRequest


                }; ;
            });
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();//default route controller>action>view
            app.UseSession();
            app.UseAuthentication();
        }
    }
}
