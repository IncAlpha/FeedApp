using System.Threading.Tasks;
using FeedApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeedApp
{
    public class Startup
    {
        private IConfigurationRoot _dbConfiguration;

        private const string DefaultConnection = "DefaultConnection";

        public Startup(IHostingEnvironment environment)
        {
            _dbConfiguration = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath).AddJsonFile("dbsettings.json").Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = _dbConfiguration.GetConnectionString(DefaultConnection);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.Run(Start);
        }

        private async Task Start(HttpContext context)
        {
            await context.Response.WriteAsync("Something is wrong!");
        }
    }
}