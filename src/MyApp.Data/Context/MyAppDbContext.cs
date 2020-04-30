using MyApp.Core;
using MyApp.Core.Configuration;
using MyApp.Core.Context;
using MyApp.Core.Data;
using MyApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace MyApp.Data.Context
{
    public class MyAppDbContext : DbContext, IDbContext
    {
        private readonly static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Core.Configuration.IConfiguration _configuration;
        private readonly IUserInformation? _userInformation;

        public ICollection<MyEntity> MyEntities { get; set; } = null!;
        public ICollection<Tenant> Tenants { get; set; } = null!;

        //public MyAppDbContext(DbContextOptions<MyAppDbContext> options, 
        //    Core.Configuration.IConfiguration configuration, IUserInformation userInformation)
        //    : base(options)
        //{
        //    _configuration = configuration;
        //    _userInformation = userInformation;
        //    //this.ChangeTracker.AutoDetectChangesEnabled = false;
        //}

        //can't inject userinformation from the integration test
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options,
            Core.Configuration.IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
            //this.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<IEntity>();

            //audit fields
            foreach (var entry in entries)
            {
                if (!(entry.Entity is IEntity entity))
                    break;

                if (entry.State == EntityState.Added)
                {
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreatedBy = _userInformation?.UserName ?? string.Empty;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdatedBy = _userInformation?.UserName;
                }
            }

            TruncateStringForChangedEntities(this);

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(MyAppDbContext)));
        }

        void IDbContext.Add(object entity)
        {
            base.Add(entity);
        }

        void IDbContext.Update(object entity)
        {
            base.Update(entity);
        }

        void IDbContext.Remove(object entity)
        {
            base.Remove(entity);
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity
        {
            return base.Set<T>().Where(filter).AsQueryable();
        }

        public string GetTableName<T>()
            where T : class, IEntity
        {
            return base.Model.FindEntityType(typeof(T)).GetTableName();
        }

        public void Execute(FormattableString sql)
        {
            //not supported by in-memory database
            if (!IsInMemoryDb)
            {
                var rawSql = string.Format(sql.Format, sql.GetArguments());
                base.Database.ExecuteSqlRaw(rawSql);
            }
            //base.Database.ExecuteSqlInterpolated(sql); //throws some @p0 exception - DJL 2/4/2020
        }

        private bool IsInMemoryDb => Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

        public bool SupportsJson() => !IsInMemoryDb && _configuration.EnvironmentName != "DEV";
        public bool SupportsBulk() => !IsInMemoryDb;

        //https://devhow.net/2019/01/17/entity-framework-core-truncating-strings-based-on-length-constraint/
        public static void TruncateStringForChangedEntities(DbContext context)
        {
            var stringPropertiesWithLengthLimitations = context.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string))
                .Select(z => new
                {
                    StringLength = z.GetMaxLength(),
                    ParentName = z.DeclaringEntityType.Name,
                    PropertyName = z.Name
                })
                .Where(d => d.StringLength.HasValue);


            var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(x => x.Entity);


            foreach (var entity in editedEntitiesInTheDbContextGraph)
            {
                var entityFields = stringPropertiesWithLengthLimitations.Where(d => d.ParentName == entity.GetType().FullName);

                foreach (var property in entityFields)
                {
                    var prop = entity.GetType().GetProperty(property.PropertyName);

                    if (prop == null)
                        continue;

                    var originalValue = prop.GetValue(entity) as string;
                    if (originalValue == null)
                        continue;

                    if (originalValue.Length > property.StringLength)
                    {
                        var entityTyped = entity as IEntity;
                        _log.Debug($"Entity '{entity.GetType().Name}':{entityTyped?.Id} Had value truncated from {originalValue.Length} to {property.StringLength} on property '{property.PropertyName}'");
                        prop.SetValue(entity, originalValue.Substring(0, property.StringLength.Value));
                    }
                }
            }
        }
    }

    public class MyAppDbContextFactory : IDesignTimeDbContextFactory<MyAppDbContext>
    {
        /// <summary>
        /// this is the endpoint used in Package Manager console. Change this to update a different database
        /// </summary>
        public MyAppDbContext CreateDbContext(string[] args)
        {
            var connString = "Data Source=localhost;Initial Catalog=MyApp;UserName=sa;Password=passwordABC123";
            var optionsBuilder = new DbContextOptionsBuilder<MyAppDbContext>();
            optionsBuilder.UseSqlServer(connString);
            return new MyAppDbContext(optionsBuilder.Options, new MockConfiguration { EnvironmentName = "DEV" });
        }
    }
}
