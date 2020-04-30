using MyApp.Core.Models;

namespace MyApp.Core.MyEntities
{
    public interface IMyEntityLoader
    {
        MyEntity? Get(long id);
    }
}