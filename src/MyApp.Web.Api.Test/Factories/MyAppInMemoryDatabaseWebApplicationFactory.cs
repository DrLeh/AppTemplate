using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MyApp.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Context;

namespace MyApp.IntegrationTest
{
    public class MyAppWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                RemoveService<DbContextOptions<MyAppDbContext>>(services);
                //RemoveService<IUserInformation>(services);
                //todo: configure custom userinfo here 

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<MyAppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetService<MyAppDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<MyAppWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
