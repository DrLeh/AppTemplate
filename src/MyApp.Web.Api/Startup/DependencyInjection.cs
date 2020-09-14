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
using MyApp.Core.Startup;
using MyApp.Data.Startup;

namespace MyApp.Web
{
    public static class DependencyInjection
    {
        public static void AddMsApi(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
        }

        public static void AddWebApi(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MyEntityProfile).Assembly);

            services.AddScoped<ITenantContext, WebTenantContext>();
            services.AddScoped<IUserInformation, WebUserInformation>();

            services.AddCore();
            services.AddData();
        }
    }
}
