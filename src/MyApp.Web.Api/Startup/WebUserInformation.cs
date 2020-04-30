using MyApp.Core.Configuration;
using MyApp.Core.Context;
using MyApp.Core.Data;
using MyApp.Core.Models;
using MyApp.Core.Security;
using MyApp.Data;
using MyApp.Data.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Web
{
    /// <summary>
    /// Gets authentication information from the <see cref="IHttpContextAccessor"/> to load information from the HttpContext
    /// </summary>
    public class WebUserInformation : IUserInformation
    {
        public WebUserInformation(IHttpContextAccessor httpContextAccessor)
        {
            UserName = httpContextAccessor.HttpContext.Request.Query["userName"].FirstOrDefault() ?? string.Empty;
        }
        public string UserName { get; }
    }


    public class WebTenantContext : ITenantContext
    {
        //inject a dbContext directly here, because IDataAccess depends on having this ITenantContext
        public WebTenantContext(IHttpContextAccessor httpContextAccessor, MyAppDbContext dbContext)
        {
            var idStr = httpContextAccessor.HttpContext.Request.Query["tenantId"].FirstOrDefault();
            if (long.TryParse(idStr, out long tenantId))
            {
                TenantId = dbContext.Tenants
                    .Where(x => x.Id == tenantId)
                    .FirstOrDefault()?.Id ?? 0;
            }
        }

        public long TenantId { get; set; }
    }
}
