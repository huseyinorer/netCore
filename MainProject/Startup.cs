using MainProject.CustomValidations;
using MainProject.Identity;
using MainProject.Models;
using MainProject.Requirement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            //claim ile filtreleme
            services.AddTransient<IAuthorizationHandler, ExpireDateExchangeHandler>();
            services.AddAuthorization(opts =>
            {

                opts.AddPolicy("KayseriPolicy", policy =>
                 {
                     policy.RequireClaim("city", "kayseri");
                     policy.RequireClaim("year", "True");
                 });

                opts.AddPolicy("ExchangePolicy", policy =>
                {
                    policy.AddRequirements(new ExpireDateExchangeRequirement());
                });

            });

            services.AddEntityFrameworkNpgsql().AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_configuration["DbConnection"]));//ders:62
            services.AddEntityFrameworkNpgsql().AddDbContext<ProjectDbContext>(options => options.UseNpgsql(_configuration["DbConnection"]));//ders:62

            //facebook ile authentication.
            services.AddAuthentication()
                .AddFacebook(opts =>
                {

                    opts.AppId = _configuration["Authentication:Facebook:AppId"];
                    opts.AppSecret = _configuration["Authentication:Facebook:AppSecret"];

                }).AddGoogle(opts =>
                {
                    opts.ClientId = _configuration["Authentication:Google:ClientID"];
                    opts.ClientSecret = _configuration["Authentication:Google:ClientSecret"];

                });
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomİdentityErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            { //ders 60
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;


            });
            //tttt
            services.AddSession();
            services.AddDistributedMemoryCache();//session nerede tutulacak : uygulmaa sunucusu hafızası
            services.ConfigureApplicationCookie(options =>
            {//ders 61
                options.LoginPath = "/Home/LogIn";
                options.LogoutPath = "/Member/Logout";
                options.AccessDeniedPath = "/Member/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".aspnet.security.cookie",
                    Path = "/",
                    SameSite = SameSiteMode.Lax,//uygulma dışından istekte bulunma
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,//http https ayarları
                    //Expiration = System.TimeSpan.FromDays(30)

                };
            });
            services.AddScoped<IClaimsTransformation, ClaimProvider.ClaimProvider>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();


            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints=>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=index}/{id?}"
                    
                    );
            });//default route controller>action>view

        }
    }
}
