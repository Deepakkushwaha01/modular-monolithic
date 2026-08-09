using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Core.Persistence.Configurations.Database.Context
{
    public interface IAppDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        ChangeTracker ChangeTracker { get; }

        void HealthCheck();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}