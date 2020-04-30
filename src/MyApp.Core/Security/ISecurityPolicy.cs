using MyApp.Core.Context;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.Security
{
    public interface ISecurityPolicy
    {
        IQueryable<T> Filter<T>(IQueryable<T> query) where T : Entity;
        IQueryable<T> FilterTenant<T>(IQueryable<T> query) where T : TenantEntity;
        void FixupEntity<T>(T entity) where T : TenantEntity;
    }

    public class PermissiveSecurityPolicy : ISecurityPolicy
    {
        public IQueryable<T> Filter<T>(IQueryable<T> query)
            where T : Entity
        {
            return query;
        }

        public IQueryable<T> FilterTenant<T>(IQueryable<T> query)
            where T : TenantEntity
        {
            return query;
        }

        public void FixupEntity<T>(T entity)
            where T : TenantEntity
        {

        }
    }

    public class TenantSecurityPolicy : ISecurityPolicy
    {
        private readonly ITenantContext _tenantContext;

        public TenantSecurityPolicy(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public IQueryable<T> Filter<T>(IQueryable<T> query)
            where T : Entity
        {
            return query;
        }

        public IQueryable<T> FilterTenant<T>(IQueryable<T> query)
            where T : TenantEntity
        {
            return query.Where(x => x.TenantId == _tenantContext.TenantId);
        }

        public void FixupEntity<T>(T entity)
            where T : TenantEntity
        {
            entity.TenantId = _tenantContext.TenantId;
        }
    }
}
