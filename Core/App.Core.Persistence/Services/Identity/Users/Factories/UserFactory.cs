using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

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
        lastLoginDate: entity.LastLoginDate, 
        profilePhoto: entity.ProfilePhoto, 
        createdOn: entity.CreatedOn, 
        updatedOn: entity.UpdatedOn);
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
        isBlocked: domain.IsBlocked, 
        isEmailVerified: domain.IsEmailVerified, 
        lastLoginDate: domain.LastLoginDate, 
        profilePhoto: domain.ProfilePhoto, 
        createdOn: domain.CreatedOn, 
        updatedOn: domain.UpdatedOn,
        createdBy: domain.CreatedBy);
}
