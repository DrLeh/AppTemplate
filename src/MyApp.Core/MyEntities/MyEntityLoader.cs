using MyApp.Core.Data;
using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp.Core.MyEntities
{
    //In orders code, the implementation of this loader had to be in the Data project
    //however, with the way we've abstracted our IDataAccess, it's not necessary to use
    // EF-specific code in the loaders.
    // Of course, it's always an option to put the loader into Data project,
    // but in many cases even the loaders should strive to not be dependent on EF-specific functions
    public class MyEntityLoader : IMyEntityLoader
    {
        private readonly IDataAccess _dataAccess;

        public MyEntityLoader(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public MyEntity? Get(long id)
        {
            return _dataAccess.QueryTenant<MyEntity>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }
    }
}
