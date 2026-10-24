using App.Core.Persistence.Configurations.Entity;

namespace App.Core.Persistence.Configurations.Database.Context.Repository
{

    public interface IRepository<TAggregate> : IDisposable where TAggregate : TrackedEntity
    {
        IQueryable<TAggregate> AllNoTrackedOf();

        void Insert(TAggregate entity);

        void Delete(TAggregate entity);
    }
}