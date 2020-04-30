using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using MyApp.Core.Security;
using MyApp.Core.Context;
using Microsoft.Extensions.Configuration;
using MyApp.Core.Configuration;
using MyApp.Core.MyEntities;

namespace MyApp.Core.Startup
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ISecurityPolicy, TenantSecurityPolicy>();
            services.AddTransient<Configuration.IConfiguration, FileConfiguration>();

            services.AddTransient<IMyEntityLoader, MyEntityLoader>();
            services.AddTransient<IMyEntityService, MyEntityService>();
        }
    }
}
