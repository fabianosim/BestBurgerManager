using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BestBurgerManager.Entities;
using BestBurgerManager.Entities.Enums;
using BestBurgerManager.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BestBurgerManagerAPI
{
    /// <summary>
    /// Main program for this API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Constructor.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the WebHostBuilder object.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>An implementation of IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
