using System.Security.Claims;
using FeedIt.Data;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeedIt
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private const string DefaultConnection = "DefaultConnection";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _configuration.GetConnectionString(DefaultConnection);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                });

            services.AddAuthorization(options => { });

            services.AddTransient<ArticlesRepository>();
            services.AddTransient<UsersRepository>();
            services.AddTransient<SubscriptionsRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Home/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseAuthentication();


            app.UseMvcWithDefaultRoute();
        }
    }
}