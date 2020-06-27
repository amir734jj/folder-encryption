using System.Security.Cryptography;
using App.Attributes;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Startup
    {
        private IWebHostEnvironment _env;
        
        private IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddRouting();

            services.AddControllersWithViews();

            // Supports ASP.Net Core DI abstractions
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;

                opt.Filters.Add<ExceptionFilterAttribute>();
            });

            services.AddLogging();
        
            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(_ =>
            {
                _.TheCallingAssembly();
                _.Assembly("Logic");
                _.WithDefaultConventions();
            });

            services.For<RijndaelManaged>().Use(new RijndaelManaged {KeySize = 256, BlockSize = 128, Padding = PaddingMode.PKCS7});
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();
            app.UseMvc();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}