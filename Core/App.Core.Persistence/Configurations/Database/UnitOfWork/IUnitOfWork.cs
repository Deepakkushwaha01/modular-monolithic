using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Persistence.Configurations.Database.Context.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// saves the current context of changes
        /// </summary>
        /// <returns></returns>
        Task<Result> SaveAsync();

        Task<Result> SaveChangesAsync();
    }
}
