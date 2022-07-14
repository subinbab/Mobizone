using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification;

using Microsoft.AspNetCore.Authentication.Cookies;
using UILayer.Data.ApiServices;

namespace UILayer1
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
            services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/denied";
                options.Events = new CookieAuthenticationEvents()
                {
                    OnSigningIn = async context =>
                    {
                        /*var principal = context.Principal;
                        if (principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                        {
                            if (principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == model.password)
                            {
                                var claimsIdentity = principal.Identity as ClaimsIdentity;
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                            }
                        }*/

                        await Task.CompletedTask;
                    },
                    OnSignedIn = async content =>
                    {
                        await Task.CompletedTask;
                    },
                    OnValidatePrincipal = async content =>
                    {
                        await Task.CompletedTask;
                    }
                };

            });
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.IsEssential = true;
            }
            );
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("defaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "Sessions";
                //options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(6);
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });
            services.AddSingleton(typeof(IUserApi), typeof(UserApi));
            services.AddSingleton(typeof(IProductOpApi), typeof(ProductOpApi));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Home/Error");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
            }
            /*app.UseHttpsRedirection();*/
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error";
                    await next();
     
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=user}/{action=Index}/{count?}");
            });
            
        }
    }
}
