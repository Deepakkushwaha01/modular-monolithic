using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Repositories;
using App.Core.Persistence.Identity.Roles.Factories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainRole = App.Core.Domain.Identity.Roles.Role;
using PersistenceRole = App.Core.Persistence.Identity.Roles.Entities.Role;

namespace App.Core.Persistence.Identity.Roles.Repositories;

public interface IRoleRepository : IRepository<PersistenceRole>
{
    Task<Result<DomainRole>> GetRoleByUidAsync(Guid uid);

    Task<Result<List<DomainRole>>> GetRolesAsync();

    Task<Result> InsertAsync(DomainRole entity);

    void Update(DomainRole entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}

public sealed class RoleRepository(IAppDbContext appDbContext)
    : Repository<PersistenceRole>(appDbContext), IRoleRepository
{
    public async Task<Result<DomainRole>> GetRoleByUidAsync(Guid uid)
    {
        try
        {
            PersistenceRole? role = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return role == null
                ? Result.NotFound<DomainRole>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(role.ToRoleDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainRole>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainRole>>> GetRolesAsync()
    {
        try
        {
            List<DomainRole> roles = await AllNoTrackedOf()
                .Select(x => x.ToRoleDomain())
                .ToListAsync();

            return Result.Ok(roles);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainRole>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainRole entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToRoleEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainRole entity)
    {
        AttachOrUpdate(entity.ToRoleEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceRole? role = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (role == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(role);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
