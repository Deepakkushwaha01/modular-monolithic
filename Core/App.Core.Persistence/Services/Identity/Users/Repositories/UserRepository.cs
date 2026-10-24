using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Repositories;
using App.Core.Persistence.Identity.Users.Factories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

namespace App.Core.Persistence.Identity.Users.Repositories;

public sealed class UserRepository(IAppDbContext appDbContext)
    : Repository<PersistenceUser>(appDbContext), IUserRepository
{
    public async Task<Result<DomainUser>> GetUserByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUser? user = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return user == null
                ? Result.NotFound<DomainUser>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(user.ToUserDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainUser>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainUser>>> GetUsersAsync()
    {
        try
        {
            List<DomainUser> users = await AllNoTrackedOf()
                .Select(x => x.ToUserDomain())
                .ToListAsync();

            return Result.Ok(users);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainUser>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return AllNoTrackedOf().AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<Result> InsertAsync(DomainUser entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToUserEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainUser entity)
    {
        AttachOrUpdate(entity.ToUserEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceUser? user = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (user == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(user);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
