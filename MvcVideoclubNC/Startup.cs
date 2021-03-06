using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcVideoclubNC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC
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

            string urlapi = 
                this.Configuration.GetValue<string>("UrlApis:ApiPeliculas");

            ServiceApiVideoclub service = new ServiceApiVideoclub(urlapi);
            services.AddTransient<ServiceApiVideoclub>
                (z => service);
            
            //Habilitamos Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();

            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);

            //Para trabajar con Blobs
            string storagekey =
                this.Configuration.GetConnectionString("AzureStorageKeys");
            services.AddTransient<ServiceStorageBlobs>(
                x => new ServiceStorageBlobs(storagekey));

            ////Usuarios con Identity

            //string appid = this.Configuration.GetValue<string>("Authentication:Microsoft:ApplicationId");
            //string secretKey = this.Configuration.GetValue<string>("Authentication:Microsoft:SecretKey");

            ////Indicamos que tendremos un usuario en dicho DbContext
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            ////Indicamos el tipo de autentificaci?n
            //services.AddAuthentication().AddMicrosoftAccount(options =>
            //{
            //    options.ClientId = appid;
            //    options.ClientSecret = secretKey;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //Para usar Session
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
