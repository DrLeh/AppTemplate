using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyApp.Core.Test
{
    //todo: move this into another class that can be shared between other test projects
    public class TestDataTransaction : IDataTransaction
    {

        public TestDataTransaction()
        {
            _commands = new List<IStoreCommand>();
        }

        public void Add<T>(T entity) where T : IEntity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Added, entity));
        }

        public void Update<T>(T entity) where T : IEntity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Updated, entity));
        }

        public void Remove<T>(T entity) where T : IEntity
        {
            _commands.Add(new TestStoreCommand<T>(StoreInteractionType.Removed, entity));
        }

        public void BulkRemove<T>(Expression<Func<T, bool>> selector) 
            where T : class, IEntity
        {
            _commands.Add(new TestBulkStoreCommand<T>(StoreInteractionType.Removed));
        }

        public void BulkUpdate<T>(Expression<Func<T, bool>> selector, Expression<Func<T, object>> obj) 
            where T : class, IEntity
        {
            _commands.Add(new TestBulkStoreCommand<T>(StoreInteractionType.Updated));
        }

        void IDataTransaction.SoftDelete<T>(Expression<Func<T, bool>> selector)
        {
            _commands.Add(new TestBulkStoreCommand<T>(StoreInteractionType.Updated));
        }

        public void Commit()
        {
            _commands.Clear();
        }

        private List<IStoreCommand> _commands;
        public IReadOnlyList<IStoreCommand> Commands => _commands;
        public IEnumerable<IEntityStoreCommand> EntityCommands => Commands.OfType<IEntityStoreCommand>();

        public IEnumerable<IStoreCommand> Added => EntityCommands.Where(x => x.Type == StoreInteractionType.Added);
        public IEnumerable<IStoreCommand> Updated => EntityCommands.Where(x => x.Type == StoreInteractionType.Updated);
        public IEnumerable<IStoreCommand> Removed => EntityCommands.Where(x => x.Type == StoreInteractionType.Removed);

        public IEnumerable<object> AddedEntity => EntityCommands.Where(x => x.Type == StoreInteractionType.Added).Select(x => x.Entity);
        public IEnumerable<object> UpdatedEntity => EntityCommands.Where(x => x.Type == StoreInteractionType.Updated).Select(x => x.Entity);
        public IEnumerable<object> RemovedEntity => EntityCommands.Where(x => x.Type == StoreInteractionType.Removed).Select(x => x.Entity);

        public IEnumerable<T> AddedEntityOfType<T>() => EntityCommands.Where(x => x.Type == StoreInteractionType.Added).Select(x => x.Entity).OfType<T>();
        public IEnumerable<T> UpdatedEntityOfType<T>() => EntityCommands.Where(x => x.Type == StoreInteractionType.Updated).Select(x => x.Entity).OfType<T>();
        public IEnumerable<T> RemovedEntityOfType<T>() => EntityCommands.Where(x => x.Type == StoreInteractionType.Removed).Select(x => x.Entity).OfType<T>();


        public IEnumerable<IBulkStoreCommand> BulkCommands => Commands.OfType<IBulkStoreCommand>();
        public IEnumerable<IBulkStoreCommand<T>> BulkCommandsOfType<T>() => Commands.OfType<IBulkStoreCommand<T>>();
        public IEnumerable<IBulkStoreCommand<T>> BulkUpdatedCommandOfType<T>() => Commands.OfType<IBulkStoreCommand<T>>().Where(x => x.Type == StoreInteractionType.Updated);
        public IEnumerable<IBulkStoreCommand<T>> BulkRemovedCommandOfType<T>() => Commands.OfType<IBulkStoreCommand<T>>().Where(x => x.Type == StoreInteractionType.Removed);


        public IEnumerable<IStoreCommand> BulkUpdated => BulkCommands.Where(x => x.Type == StoreInteractionType.Updated);
        public IEnumerable<IStoreCommand> BulkRemoved => BulkCommands.Where(x => x.Type == StoreInteractionType.Removed);
    }
}

