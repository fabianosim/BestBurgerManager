using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BestBurgerManager.Business;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using BestBurgerManagerAPI.ApiResponses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BestBurgerManagerAPI
{
    /// <summary>
    /// API's Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The environment for the running API.
        /// </summary>
        public IWebHostEnvironment _environment { get; }
        
        /// <summary>
        /// The logger factory implementation.
        /// </summary>
        public ILoggerFactory _loggerFactory { get; }

        /// <summary>
        /// The Configuration object dependency. Contains configuration properties about the application.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Entry point for Startup class.
        /// </summary>
        /// <param name="environment">An implementation of the environment</param>
        /// <param name="loggerFactory">An implementation of the logger factory</param>
        /// <param name="configuration">An implementation of the configuration object</param>
        public Startup(IWebHostEnvironment environment, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _environment = environment;
            _loggerFactory = loggerFactory;
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The Service Collection implementation.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiValidationFilter));
                options.EnableEndpointRouting = false;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            // Register required services.
            services.RegisterServices(env: _environment);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The Application Builder implementation</param>
        /// <param name="env">The Environment Implementation</param>
        /// <param name="userManager">The UserManager implementation. Populates the fixed users for this API.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserManager userManager)
        {
            // Initialize API users
            using (StreamReader r = new StreamReader("./Data/apiusers.json"))
            {
                string jsonApiUsers = r.ReadToEnd();
                var apiUsers = JsonConvert.DeserializeObject<List<User>>(jsonApiUsers);

                foreach (User apiUser in apiUsers)
                {
                    userManager.AddUser(apiUser);
                }
            }
                
            //userManager.AddUser(user);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();
            app.UseMvc();

            // Swagger documentation middlewares
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Best Burger Management API");
            });
        }
    }
}
