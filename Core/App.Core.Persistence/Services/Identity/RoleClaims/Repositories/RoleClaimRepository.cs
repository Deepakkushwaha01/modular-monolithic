using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Claims.Factories;
using App.Core.Persistence.Identity.Repositories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainRoleClaim = App.Core.Domain.Identity.Claims.RoleClaim;
using PersistenceRoleClaim = App.Core.Persistence.Identity.Claims.Entities.RoleClaim;

namespace App.Core.Persistence.Identity.Claims.Repositories;

public interface IRoleClaimRepository : IRepository<PersistenceRoleClaim>
{
    Task<Result<DomainRoleClaim>> GetRoleClaimByUidAsync(Guid uid);

    Task<Result<List<DomainRoleClaim>>> GetRoleClaimsAsync();

    Task<Result> InsertAsync(DomainRoleClaim entity);

    void Update(DomainRoleClaim entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}

public sealed class RoleClaimRepository(IAppDbContext appDbContext)
    : Repository<PersistenceRoleClaim>(appDbContext), IRoleClaimRepository
{
    public async Task<Result<DomainRoleClaim>> GetRoleClaimByUidAsync(Guid uid)
    {
        try
        {
            PersistenceRoleClaim? roleClaim = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return roleClaim == null
                ? Result.NotFound<DomainRoleClaim>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(roleClaim.ToRoleClaimDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainRoleClaim>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainRoleClaim>>> GetRoleClaimsAsync()
    {
        try
        {
            List<DomainRoleClaim> roleClaims = await AllNoTrackedOf()
                .Select(x => x.ToRoleClaimDomain())
                .ToListAsync();

            return Result.Ok(roleClaims);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainRoleClaim>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainRoleClaim entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToRoleClaimEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainRoleClaim entity)
    {
        AttachOrUpdate(entity.ToRoleClaimEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceRoleClaim? roleClaim = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (roleClaim == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(roleClaim);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
