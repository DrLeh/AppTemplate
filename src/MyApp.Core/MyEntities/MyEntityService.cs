using MyApp.Core.Data;
using MyApp.Core.Models;
using MyApp.Core.MyEntities.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.MyEntities
{
    public class MyEntityService : IMyEntityService
    {
        private readonly IDataAccess _dataAccess;
        private readonly IMyEntityLoader _myEntityLoader;

        public MyEntityService(IDataAccess dataAccess, IMyEntityLoader myEntityLoader)
        {
            _dataAccess = dataAccess;
            _myEntityLoader = myEntityLoader;
        }

        public MyEntity? Get(long id)
        {
            return _myEntityLoader.Get(id);
        }

        /// <summary>
        /// Simple scenario that doesn't really need unit testing
        /// </summary>
        public MyEntity Add(MyEntity toAdd)
        {
            var trans = _dataAccess.CreateTransaction();
            trans.Add(toAdd);
            trans.Commit();
            return toAdd;
        }

        /// <summary>
        /// More compex scenario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public MyEntity Update(long id, Action<MyEntity> update)
        {
            var trans = _dataAccess.CreateTransaction();
            var context = new MyEntityUpdateContext(id, update, _myEntityLoader, trans);
            var op = new MyEntityUpdateOperation(context);

            op.Load();
            op.StageChanges();

            trans.Commit();
            //if we handle the not found case, we can assert that context.Entity will have a value here.
            return context.Entity!;
        }
    }
}
