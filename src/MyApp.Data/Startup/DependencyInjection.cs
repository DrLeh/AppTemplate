using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyApp.Data.Context;
using Microsoft.Extensions.Configuration;

namespace MyApp.Data.Startup
{
    public static class DependencyInjection
    {
        public static void AddData(this IServiceCollection services)
        {
            services.AddDbContext<MyAppDbContext>((sp, options) => options.UseSqlServer(sp
                .GetService<Core.Configuration.IConfiguration>().ConnectionString));

            services.AddScoped<IDataAccess, DataAccess>();
        }
    }
}
