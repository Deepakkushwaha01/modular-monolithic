using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Repositories;
using App.Core.Persistence.Identity.Roles.Factories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainUserRole = App.Core.Domain.Identity.Roles.UserRole;
using PersistenceUserRole = App.Core.Persistence.Identity.Roles.Entities.UserRole;

namespace App.Core.Persistence.Identity.Roles.Repositories;

public interface IUserRoleRepository : IRepository<PersistenceUserRole>
{
    Task<Result<DomainUserRole>> GetUserRoleByUidAsync(Guid uid);

    Task<Result<List<DomainUserRole>>> GetUserRolesAsync();

    Task<Result> InsertAsync(DomainUserRole entity);

    void Update(DomainUserRole entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}

public sealed class UserRoleRepository(IAppDbContext appDbContext)
    : Repository<PersistenceUserRole>(appDbContext), IUserRoleRepository
{
    public async Task<Result<DomainUserRole>> GetUserRoleByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUserRole? userRole = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return userRole == null
                ? Result.NotFound<DomainUserRole>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(userRole.ToUserRoleDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainUserRole>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainUserRole>>> GetUserRolesAsync()
    {
        try
        {
            List<DomainUserRole> userRoles = await AllNoTrackedOf()
                .Select(x => x.ToUserRoleDomain())
                .ToListAsync();

            return Result.Ok(userRoles);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainUserRole>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainUserRole entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToUserRoleEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainUserRole entity)
    {
        AttachOrUpdate(entity.ToUserRoleEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUserRole? userRole = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (userRole == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(userRole);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
