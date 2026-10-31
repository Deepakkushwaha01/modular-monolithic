using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Repositories;
using App.Core.Persistence.Identity.Users.Factories;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceRole = App.Core.Persistence.Identity.Roles.Entities.Role;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

namespace App.Core.Persistence.Identity.Users.Repositories;

public sealed class UserRepository(IAppDbContext appDbContext)
    : Repository<PersistenceUser>(appDbContext), IUserRepository
{
    public async Task<Result<DomainUser>> GetUserWithRolesAndClaimsAsync(string email)
    {
        try
        {
            string normalizedEmail = email.Trim().ToLowerInvariant();

            PersistenceUser? user = await AllNoTrackedOf()
                .Include(entity => entity.UserRoles)
                    .ThenInclude(userRole => userRole.Role)
                        .ThenInclude(role => role.RoleClaims)
                            .ThenInclude(roleClaim => roleClaim.ClaimType)
                .Include(entity => entity.UserClaims)
                    .ThenInclude(userClaim => userClaim.ClaimType)
                .SingleOrDefaultAsync(entity =>
                    entity.Email.ToLower() == normalizedEmail &&
                    entity.IsActive &&
                    !entity.IsBlocked);

            if (user is null)
            {
                return Result.NotFound<DomainUser>(PersistenceErrorMessages.EntityNotFound);
            }

            List<PersistenceRole> activeRoles = user.UserRoles
                .Select(userRole => userRole.Role)
                .Where(role => role is { IsActive: true })
                .DistinctBy(role => role.Uid)
                .ToList();

            List<string> claims = user.UserClaims
                .Where(userClaim => userClaim.ClaimType is { IsActive: true } && bool.Parse(userClaim.ClaimValue))
                .Select(userClaim => userClaim.ClaimType.Name)
                .ToList();

            claims.AddRange(
                activeRoles
                    .SelectMany(role => role.RoleClaims)
                    .Where(roleClaim => roleClaim.ClaimType is { IsActive: true } && bool.Parse(roleClaim.ClaimValue))
                    .Select(roleClaim => roleClaim.ClaimType.Name));

            DomainUser domainUser = user.ToUserDomain();
            domainUser.AddRoles(activeRoles.Select(role => role.Slug));
            domainUser.AddClaims(claims);

            return Result.Ok(domainUser);
        }
        catch (Exception)
        {
            return Result.Failed<DomainUser>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> RecordSuccessfulLoginAsync(Guid uid)
    {
        try
        {
            PersistenceUser? user = await _dbSet.FirstOrDefaultAsync(entity => entity.Uid == uid);
            if (user is null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            user.LastLoginDate = DateTime.UtcNow;

            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<DomainUser>> GetUserByUidAsync(
        Guid uid,
        bool includeAddresses = false,
        bool includeRoles = false,
        bool includeClaims = false)
    {
        try
        {
            IQueryable<PersistenceUser> query = AllNoTrackedOf();

            if (includeAddresses)
            {
                query = query.Include(x => x.Addresses);
            }

            if (includeRoles)
            {
                query = query.Include(x => x.UserRoles);
            }

            if (includeClaims)
            {
                query = query.Include(x => x.UserClaims);
            }

            PersistenceUser? user = await query.FirstOrDefaultAsync(x => x.Uid == uid);

            return user is null
                ? Result.NotFound<DomainUser>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(user.ToUserDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainUser>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainUser>>> GetUsersAsync(
        bool includeAddresses = false,
        bool includeRoles = false,
        bool includeClaims = false)
    {
        try
        {
            IQueryable<PersistenceUser> query = AllNoTrackedOf();

            if (includeAddresses)
            {
                query = query.Include(x => x.Addresses);
            }

            if (includeRoles)
            {
                query = query.Include(x => x.UserRoles);
            }

            if (includeClaims)
            {
                query = query.Include(x => x.UserClaims);
            }

            List<PersistenceUser> entities = await query.ToListAsync();
            List<DomainUser> users = entities.Select(x => x.ToUserDomain()).ToList();

            return Result.Ok(users);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainUser>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        string normalizedEmail = email.Trim().ToLowerInvariant();
        return AllNoTrackedOf().AnyAsync(x => x.Email.ToLower() == normalizedEmail);
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
            if (user is null)
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