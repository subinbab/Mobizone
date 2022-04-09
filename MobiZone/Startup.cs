using ApiLayer.DI;
using ApiLayer.Models;
using BusinessObjectLayer;
using BusinessObjectLayer.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobiZone
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            addSerivices = new AddSerivices();
        }

        public IConfiguration Configuration { get; }
        public AddSerivices addSerivices { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("myoffice")));
            /*services.AddScoped(typeof(IRepositoryOperations<>), typeof(RepositoryOperations<>));*/
            services.AddScoped(typeof(IProductCatalog), typeof(ProductCatalog));
            services.AddScoped(typeof(IUserCreate), typeof(UserCreate));
            services.AddScoped(typeof(IRepositoryOperations<>), typeof(RepositoryOperations<>));
            addSerivices.Initialize(services);
            services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            /*app.UseHttpsRedirection();*/

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Product}/{action=Index}/{id?}");
            });
        }
    }
}
