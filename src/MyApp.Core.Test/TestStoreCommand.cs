using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyApp.Core.Test
{
    public class TestStoreCommand<T> : IEntityStoreCommand<T>
        where T : IEntity
    {
        public StoreInteractionType Type { get; }
        public object Entity { get; }
        public T EntityTyped { get; }

        public TestStoreCommand(StoreInteractionType type, T entity)
        {
            Type = type;
            Entity = entity;
            EntityTyped = entity;
        }

        public void Execute(IDbContext context)
        {
            switch (Type)
            {
                case StoreInteractionType.Added: context.Add(Entity); break;
                case StoreInteractionType.Updated: context.Update(Entity); break;
                case StoreInteractionType.Removed: context.Remove(Entity); break;
            }
        }
    }
    public class TestBulkStoreCommand<T> : IBulkStoreCommand<T>
        where T : IEntity
    {
        public Expression<Func<T, bool>> Selector => throw new NotImplementedException();
        public StoreInteractionType Type { get; }

        public TestBulkStoreCommand(StoreInteractionType type)
        {
            Type = type;
        }


        public void Execute(IDbContext context)
        {

        }
    }
}
