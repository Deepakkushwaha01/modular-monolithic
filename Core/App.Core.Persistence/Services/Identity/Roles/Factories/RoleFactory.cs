using DomainRole = App.Core.Domain.Identity.Roles.Role;
using PersistenceRole = App.Core.Persistence.Identity.Roles.Entities.Role;
using App.Core.Persistence.Identity.Claims.Factories;

namespace App.Core.Persistence.Identity.Roles.Factories;

public static class RoleFactory
{
    public static DomainRole ToRoleDomain(this PersistenceRole entity) => DomainRole.Create(entity.Id, entity.Uid, entity.Name,
        entity.Slug, entity.Description, entity.IsSystemRole, entity.IsActive, entity.CreatedOn, entity.UpdatedOn,
        entity.UserRoles.Select(x => x.ToUserRoleDomain()), entity.RoleClaims.Select(x => x.ToRoleClaimDomain()));

    public static PersistenceRole ToRoleEntity(this DomainRole domain) => PersistenceRole.Create(domain.Id, domain.Uid, domain.Name,
        domain.Slug, domain.Description, domain.IsSystemRole, domain.IsActive, domain.CreatedOn, domain.UpdatedOn);
}
