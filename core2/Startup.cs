using core2.Identity;
using core2.Model;
using core2.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace core2
{

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddEntityFrameworkNpgsql().AddDbContext<SchoolContext>(options => options.UseNpgsql(_configuration["DbConnection"]));//ders:57
            services.AddEntityFrameworkNpgsql().AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_configuration["DbConnection"]));//ders:62
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            { //ders 60
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });
            services.AddScoped<ICalculate, Calculator8>();//transient ve singleton var 2 nesne alması durumu veya alınan değişkenler için tek nesye veya farklı nesne oluşturma durumu
            services.AddSession();
            services.AddDistributedMemoryCache();//session nerede tutulacak : uygulmaa sunucusu hafızası
            services.ConfigureApplicationCookie(options =>
            {//ders 61
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                options.AccessDeniedPath = "/Security/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".aspnet.security.cookie2",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,//uygulma dışından istekte bulunma
                    SecurePolicy = CookieSecurePolicy.SameAsRequest


                }; ;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {

            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=persons}/{action=index}/{id?}"
                    );
            endpoints.MapControllerRoute(
                   name: "Myroute",
                   pattern: "Engin/{controller=Home}/{action=index2}/{id?}"
                    );
            endpoints.MapControllerRoute(
                   name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
        });//default route controller>action>view

        }

        private void configureRoutes(IRouteBuilder obj)
        {
            obj.MapRoute("Default", "{controller=persons}/{action=index}/{id?}");
            obj.MapRoute("Myroute", "Engin/{controller=Home}/{action=index2}/{id?}");
            obj.MapRoute(name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                            );
        }
    }
}
