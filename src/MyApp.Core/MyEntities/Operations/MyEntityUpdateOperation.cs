using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.MyEntities.Operations
{
    public class MyEntityUpdateContext
    {
        public MyEntityUpdateContext(long id, Action<MyEntity> updateFunc, IMyEntityLoader loader, IDataTransaction dataTransaction)
        {
            Id = id;
            UpdateFunc = updateFunc;
            Loader = loader;
            DataTransaction = dataTransaction;
        }

        public long Id { get; }
        public Action<MyEntity> UpdateFunc { get; }
        public IMyEntityLoader Loader { get; }
        public IDataTransaction DataTransaction { get; }

        public MyEntity? Entity { get; set; }
    }

    public class MyEntityUpdateOperation
    {
        public MyEntityUpdateOperation(MyEntityUpdateContext context)
        {
            Context = context;
        }

        public MyEntityUpdateContext Context { get; }

        public void Load()
        {
            Context.Entity = Context.Loader.Get(Context.Id);
        }

        public void StageChanges()
        {
            if (Context.Entity != null)
            {
                Context.UpdateFunc(Context.Entity);

                //here would be some more complex logic like updating child objects or what have you... 
                // things that need to be unit tested.

                Context.DataTransaction.Update(Context.Entity);
            }

            //else: handle error or throw? depends on the scenario
        }
    }
}
