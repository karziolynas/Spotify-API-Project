using API_project.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_project
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
            //used to "prevent" ddos attacks 
            #region AspNetCoreRateLimit
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            services.AddInMemoryRateLimiting();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion AspNetCoreRateLimit

            services.AddHttpClient<ISpotifyServiceAccount, SpotifyServiceAccount>(c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            services.AddHttpClient<ISpotifyService, SpotifyService>(c =>
            {
                c.BaseAddress = new Uri("https://api.spotify.com/v1/");
                c.DefaultRequestHeaders.Add("Accept", "application/.json");
            });

            //services.AddHttpClient<ISpotifyAudioFeaturesService, SpotifyAudioFeaturesService>(c =>
            //{
            //    c.BaseAddress = new Uri("https://api.spotify.com/v1/");
            //    c.DefaultRequestHeaders.Add("Accept", "application/.json");
            //});

            services.AddHttpClient<ISpotifyTopArtistsService, SpotifyTopArtistsService>(c =>
            {
                c.BaseAddress = new Uri("https://api.spotify.com/v1/me/");
                c.DefaultRequestHeaders.Add("Accept", "application/.json");
            });

            services.AddHttpClient<IChartService, ChartService>(c =>
            {
                c.BaseAddress = new Uri("https://quickchart.io/");
            });

            services.AddHttpClient<ITasteDiveService, TasteDiveService>();

            


            services.AddControllersWithViews();
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
            app.UseIpRateLimiting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
           
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}