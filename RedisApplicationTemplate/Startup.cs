using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeCampCacheLib;
using ExampleWebsiteRedis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WeatherSdk;

namespace RedisApplicationTemplate
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // We will inject the new Fibonacci service (with caching) and disable the old one
            //services.AddTransient<IValueService, ValueService>();
            services.AddTransient<IValueService, RedisValueService>();

            // We will inject the new Weather service(with caching) and disable the old one
            //services.AddTransient<IWeatherService, WeatherService>();
            services.AddTransient<IWeatherService, RedisWeatherService>();

            // Add built-in distributed caching to the application... SOOOOOO easy!!
            services.AddDistributedRedisCache(options => { options.Configuration = "127.0.0.1:6379"; });
            
            // Adding StackExchange.Redis
            IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints =
                {
                    { "127.0.0.1", 6379 }
                },
                AbortOnConnectFail = false                
            });

            services.AddSingleton(multiplexer);

            services.AddTransient<ICardService, CardService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
