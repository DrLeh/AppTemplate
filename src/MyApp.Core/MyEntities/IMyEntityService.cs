using MyApp.Core.Models;
using System;

namespace MyApp.Core.MyEntities
{
    public interface IMyEntityService
    {
        MyEntity Add(MyEntity toAdd);
        MyEntity? Get(long id);
        MyEntity Update(long id, Action<MyEntity> update);
    }
}