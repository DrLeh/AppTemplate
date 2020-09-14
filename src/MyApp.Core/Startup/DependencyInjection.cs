using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using MyApp.Core.Security;
using MyApp.Core.Configuration;
using MyApp.Core.MyEntities;
using System.Linq;

namespace MyApp.Core.Startup
{
    public static class DependencyInjection
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddScoped<ISecurityPolicy, TenantSecurityPolicy>();
            services.AddScoped<Configuration.IConfiguration, FileConfiguration>();

            services.AddScoped<IMyEntityLoader, MyEntityLoader>();
            services.AddScoped<IMyEntityService, MyEntityService>();
        }

        public static void AssertConfigurationIsValid(this IServiceCollection services)
        {
            var exceptions = new List<Exception>();
            var prov = services.BuildServiceProvider();
            foreach (var service in services)
            {
                try
                {
                    prov.GetService(service.ServiceType);
                }
                catch (InvalidOperationException e)
                {
                    exceptions.Add(e);
                }
                catch (ArgumentException e)
                {
                    //so far only MS code is throwing Argument exception, ignore these as they 
                    // are not relevant
                    //exceptions.Add(e);
                }
            }
            if (exceptions.Any())
            {
                throw new AggregateException("Some services are missing", exceptions);
            }
        }

        public static void EjectAllInstancesOf<T>(this IServiceCollection c)
        {
            ServiceDescriptor serviceDescriptor;
            do
            {
                serviceDescriptor = c.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
                if (serviceDescriptor != null)
                    c.Remove(serviceDescriptor);
            }
            while (serviceDescriptor != null);
        }
    }
}
