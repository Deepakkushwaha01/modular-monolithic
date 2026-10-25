using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Claims.Factories;
using App.Core.Persistence.Identity.Repositories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainClaimType = App.Core.Domain.Identity.Claims.ClaimType;
using PersistenceClaimType = App.Core.Persistence.Identity.Claims.Entities.ClaimType;

namespace App.Core.Persistence.Identity.Claims.Repositories;

public interface IClaimTypeRepository : IRepository<PersistenceClaimType>
{
    Task<Result<DomainClaimType>> GetClaimTypeByUidAsync(Guid uid, bool includeRoleClaims = false, bool includeUserClaims = false);

    Task<Result<List<DomainClaimType>>> GetClaimTypesAsync(bool includeRoleClaims = false, bool includeUserClaims = false);

    Task<Result> InsertAsync(DomainClaimType entity);

    void Update(DomainClaimType entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}

public sealed class ClaimTypeRepository(IAppDbContext appDbContext)
    : Repository<PersistenceClaimType>(appDbContext), IClaimTypeRepository
{
    public async Task<Result<DomainClaimType>> GetClaimTypeByUidAsync(Guid uid, bool includeRoleClaims = false, bool includeUserClaims = false)
    {
        try
        {
            IQueryable<PersistenceClaimType> query = AllNoTrackedOf();
            if (includeRoleClaims) query = query.Include(x => x.RoleClaims);
            if (includeUserClaims) query = query.Include(x => x.UserClaims);
            PersistenceClaimType? claimType = await query.FirstOrDefaultAsync(x => x.Uid == uid);
            return claimType == null
                ? Result.NotFound<DomainClaimType>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(claimType.ToClaimTypeDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainClaimType>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainClaimType>>> GetClaimTypesAsync(bool includeRoleClaims = false, bool includeUserClaims = false)
    {
        try
        {
            IQueryable<PersistenceClaimType> query = AllNoTrackedOf();
            if (includeRoleClaims) query = query.Include(x => x.RoleClaims);
            if (includeUserClaims) query = query.Include(x => x.UserClaims);
            List<PersistenceClaimType> entities = await query.ToListAsync();
            List<DomainClaimType> claimTypes = entities.Select(x => x.ToClaimTypeDomain()).ToList();

            return Result.Ok(claimTypes);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainClaimType>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainClaimType entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToClaimTypeEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainClaimType entity)
    {
        AttachOrUpdate(entity.ToClaimTypeEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceClaimType? claimType = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (claimType == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(claimType);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
