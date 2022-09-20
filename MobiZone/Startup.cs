using ApiLayer.DI;
using ApiLayer.GraphQl;
using ApiLayer.Models;
using BusinessObjectLayer;
using BusinessObjectLayer.User;
using DomainLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;

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
            services.AddIdentity<IdentityUser, IdentityRole>(
        options => {
            options.SignIn.RequireConfirmedAccount = false;

            //Other options go here
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient(typeof(IMailService), typeof(MailService));
        }
        )
    .AddEntityFrameworkStores<ProductDbContext>();
            services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
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
            services.AddGraphQLServer().AddQueryType<Query>();

            /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });*/

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
                /*endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}");*/
                endpoints.MapGraphQL();
            });
        }
    }
}
