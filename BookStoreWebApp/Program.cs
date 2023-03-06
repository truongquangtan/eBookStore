using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BookStoreWebApp.Supporters.DataGenerator;
using Microsoft.Extensions.DependencyInjection;
using BookStoreWebApp.Models;

namespace BookStoreWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var app = CreateHostBuilder(args).Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<eBookStore5Context>();
                context.Database.EnsureCreated();

                var adminPassword = config.GetValue<string>("AdminAccount:password");
                var adminName = config.GetValue<string>("AdminAccount:username");

                await ApplicationDataGenerator.Initialize(services, adminName, adminPassword);

            }
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
