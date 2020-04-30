using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Data.Mappings
{
    public abstract class EntityMapping<T> : IEntityTypeConfiguration<T>
        where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.CreatedBy).HasMaxLength(EntityMetadata.UserMaxLength).HasDefaultValueSql("((SUSER_SNAME()+' Proc=')+ISNULL(OBJECT_NAME(@@procid),''))");
            b.Property(x => x.UpdatedBy).HasMaxLength(EntityMetadata.UserMaxLength);

            b.Property(x => x.CreateDate).HasColumnName("CreateDateUTC").HasDefaultValueSql("GETUTCDATE()");
            b.Property(x => x.UpdateDate).HasColumnName("UpdateDateUTC");

            ConfigureInternal(b);
        }

        protected abstract void ConfigureInternal(EntityTypeBuilder<T> b);
    }

    public abstract class TenantEntityMapping<T> : EntityMapping<T>
        where T : TenantEntity
    {
        public override void Configure(EntityTypeBuilder<T> b)
        {
            base.Configure(b);

            ConfigureInternal(b);
        }
    }

    public class MyEntityMapping : TenantEntityMapping<MyEntity>
    {
        protected override void ConfigureInternal(EntityTypeBuilder<MyEntity> b)
        {
        }
    }
}
