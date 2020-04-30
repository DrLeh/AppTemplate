using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Data;
using MyApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Core.Context;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using MyApp.Core.Mappers;

namespace MyApp.Web
{
    public class Registry
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddRazorPages()
            //.AddRazorPagesOptions(o =>
            //{
            //    o.Conventions.AddPageRoute("/Model/{name}", "invoices");
            //})
            ;

            services.AddAutoMapper(typeof(MyEntityProfile).Assembly);
            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddTransient<ITenantContext, WebTenantContext>();
            services.AddTransient<IUserInformation, WebUserInformation>();

            new Core.Startup.Registry().RegisterServices(services);
            new Data.Startup.Registry().RegisterServices(services);
        }
    }
}
