using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Claims.Factories;
using App.Core.Persistence.Identity.Repositories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainUserClaim = App.Core.Domain.Identity.Claims.UserClaim;
using PersistenceUserClaim = App.Core.Persistence.Identity.Claims.Entities.UserClaim;

namespace App.Core.Persistence.Identity.Claims.Repositories;

public interface IUserClaimRepository : IRepository<PersistenceUserClaim>
{
    Task<Result<DomainUserClaim>> GetUserClaimByUidAsync(Guid uid);

    Task<Result<List<DomainUserClaim>>> GetUserClaimsAsync();

    Task<Result> InsertAsync(DomainUserClaim entity);

    void Update(DomainUserClaim entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}

public sealed class UserClaimRepository(IAppDbContext appDbContext)
    : Repository<PersistenceUserClaim>(appDbContext), IUserClaimRepository
{
    public async Task<Result<DomainUserClaim>> GetUserClaimByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUserClaim? userClaim = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return userClaim == null
                ? Result.NotFound<DomainUserClaim>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(userClaim.ToUserClaimDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainUserClaim>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainUserClaim>>> GetUserClaimsAsync()
    {
        try
        {
            List<DomainUserClaim> userClaims = await AllNoTrackedOf()
                .Select(x => x.ToUserClaimDomain())
                .ToListAsync();

            return Result.Ok(userClaims);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainUserClaim>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainUserClaim entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToUserClaimEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainUserClaim entity)
    {
        AttachOrUpdate(entity.ToUserClaimEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUserClaim? userClaim = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (userClaim == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(userClaim);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
