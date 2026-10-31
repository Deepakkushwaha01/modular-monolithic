using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;
using App.Core.Persistence.Identity.Addresses.Factories;
using App.Core.Persistence.Identity.Roles.Factories;
using App.Core.Persistence.Identity.Claims.Factories;

namespace App.Core.Persistence.Identity.Users.Factories;

public static class UserFactory
{
    public static DomainUser ToUserDomain(this PersistenceUser entity)
    {
        return DomainUser.Create(
        id: entity.Id,
        uid: entity.Uid,
        email: entity.Email,
        password: entity.Password,
        firstName: entity.FirstName,
        lastName: entity.LastName,
        phone: entity.Phone,
        isPhoneVerified: entity.IsPhoneVerified,
        isActive: entity.IsActive,
        isBlocked: entity.IsBlocked,
        isEmailVerified: entity.IsEmailVerified,
        portal: entity.Portal,
        lastLoginDate: entity.LastLoginDate,
        profilePhoto: entity.ProfilePhoto,
        createdOn: entity.CreatedOn,
        updatedOn: entity.UpdatedOn,
        createdBy: entity.CreatedBy,
        updatedBy: entity.UpdatedBy,
        addresses: entity.Addresses.Select(x => x.ToAddressDomain()),
        userRoles: entity.UserRoles.Select(x => x.ToUserRoleDomain()),
        userClaims: entity.UserClaims.Select(x => x.ToUserClaimDomain()));
    }

    public static PersistenceUser ToUserEntity(this DomainUser domain) => PersistenceUser.Create(
        id: domain.Id,
        uid: domain.Uid,
        email: domain.Email,
        password: domain.Password,
        firstName: domain.FirstName,
        lastName: domain.LastName,
        phone: domain.Phone,
        isPhoneVerified: domain.IsPhoneVerified,
        isActive: domain.IsActive,
        portal: domain.Portal,
        isBlocked: domain.IsBlocked,
        isEmailVerified: domain.IsEmailVerified,
        lastLoginDate: domain.LastLoginDate,
        profilePhoto: domain.ProfilePhoto,
        createdOn: domain.CreatedOn,
        updatedOn: domain.UpdatedOn,
        createdBy: domain.CreatedBy,
        updatedBy: domain.UpdatedBy);
}
