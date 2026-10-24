using App.Core.Persistence.Configurations.Entity;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Persistence.Configurations.Database.Context.Repository
{

    public class Repository<TAggregate> : IRepository<TAggregate>
        where TAggregate : TrackedEntity
    {
        protected readonly IAppDbContext _appDbContext;

        protected readonly DbSet<TAggregate> _dbSet;

        public Repository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<TAggregate>();
        }

        public virtual IQueryable<TAggregate> AllNoTrackedOf()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual void Insert(TAggregate entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(TAggregate entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<Result> SaveChangesAsync()
        {
            try
            {
                await _appDbContext.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Failed(ex.Message);
            }
        }

        protected void AttachOrUpdate<TEntity>(TEntity entity, EntityState entityState)
            where TEntity : TrackedEntity
        {
            TEntity? existingEntity =
                entityState == EntityState.Added
                    ? null
                    : _appDbContext
                        .Set<TEntity>()
                        .Local.SingleOrDefault(e => e.Id == entity.Id);

            if (existingEntity == null)
            {
                _appDbContext.Entry(entity).State = entityState;
            }
            else
            {
                _appDbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                _appDbContext.Entry(entity).State = EntityState.Detached;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}
