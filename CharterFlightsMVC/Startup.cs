﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharterFlightsMVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharterFlightsMVC
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

            services.AddTransient<ValidateAccessTokenHandler>();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IAmadeusService, AmadeusService>();
            //.AddHttpMessageHandler<ValidateAccessTokenHandler>();


            services.AddResponseCaching();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(MyCustomMiddleware);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseResponseCaching();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Flights}/{action=Index}/{id?}");
            });
        }

        private RequestDelegate MyCustomMiddleware(RequestDelegate next)
        {            
            return async ctx =>
            {
                if (ctx.Request.Path.StartsWithSegments("/hello"))
                    await ctx.Response.WriteAsync("Hello, World!");
                else
                    await next(ctx);
            };
        }
    }
}
