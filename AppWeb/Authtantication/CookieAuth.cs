using AppRepository.Context;
using AppRepository.Entities;
using AppServices.Describer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Authtantication
{
    public static class CookieAuth
    {
        public static IServiceCollection AddCookieAuth(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddErrorDescriber<UserIdentityErrorDescriber>()
             .AddEntityFrameworkStores<AuthtakeDbContext>()
             .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Index";
                options.LogoutPath = "/Logout/Index";
                options.AccessDeniedPath = new PathString("/ErrorPage/Index");
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "AuthakeCookie";
            });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            return services;
        }
    }
}
