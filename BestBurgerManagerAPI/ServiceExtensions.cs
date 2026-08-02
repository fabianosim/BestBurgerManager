using BestBurgerManager.Business;
using BestBurgerManager.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BestBurgerManagerAPI
{
    /// <summary>
    /// Class that extends the Service registration implementation. Just to make things clearer when setting up the services for this API.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// The registered platform services reference.
        /// </summary>
        public static object PlatformServices { get; private set; }

        /// <summary>
        /// Extension method to register multiple services for the API.
        /// </summary>
        /// <param name="services">The service collection instance.</param>
        /// <param name="env">The environment containing running application configuration.</param>
        /// <returns>Returns the service collection containing all registered services.</returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            /***
             * We are adding as a Singleton to persist orders and users to the entire application lifecycle
            */
            services.AddSingleton<IUserManager, UserManager>();
            services.AddSingleton<IOrderManager, OrderManager>();

            // This service does not need to be set to Singleton. No state will be hold for any object on this service.
            services.AddTransient<IProductManager, ProductManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Best Burger Management API",
                        Version = "v1.0.0",
                        Description = "API to route orders from each POS to the main Kitchen.",
                        Contact = new OpenApiContact
                        {
                            Name = "RDI/Capgemini Developer Candidate",
                            Url = new Uri("https://github.com/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "This API is under the GPL 3.0 license.",
                            Url = new Uri("https://opensource.org/licenses/GPL-3.0")
                        }
                    });

                string appPath = env.ContentRootPath;
                string nomeAplicacao = env.ApplicationName;
                string xmlDocPath = Path.Combine(appPath, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(xmlDocPath);
            });

            return services;
        }
    }
}
