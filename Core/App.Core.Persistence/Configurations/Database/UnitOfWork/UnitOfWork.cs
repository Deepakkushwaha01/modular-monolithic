using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace App.Core.Persistence.Configurations.Database.Context.UnitOfWork
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext _appDbContext;

        public UnitOfWork(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<Result> SaveAsync()
        {
            return SaveChanges();
        }

        private async Task<Result> SaveChanges()
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

        public async Task<Result> SaveChangesAsync()
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
    }
}
