using DomainUserRole = App.Core.Domain.Identity.Roles.UserRole;
using PersistenceUserRole = App.Core.Persistence.Identity.Roles.Entities.UserRole;

namespace App.Core.Persistence.Identity.Roles.Factories;

public static class UserRoleFactory
{
    public static DomainUserRole ToUserRoleDomain(this PersistenceUserRole entity) => DomainUserRole.Create(entity.Id, entity.Uid,
        entity.UserId, entity.RoleId, entity.AssignedAt, entity.AssignedBy, entity.CreatedOn, entity.UpdatedOn);

    public static PersistenceUserRole ToUserRoleEntity(this DomainUserRole domain) => PersistenceUserRole.Create(domain.Id, domain.Uid,
        domain.UserId, domain.RoleId, domain.AssignedAt, domain.AssignedBy, domain.CreatedOn, domain.UpdatedOn);
}
