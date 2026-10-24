using DomainRoleClaim = App.Core.Domain.Identity.Claims.RoleClaim;
using PersistenceRoleClaim = App.Core.Persistence.Identity.Claims.Entities.RoleClaim;

namespace App.Core.Persistence.Identity.Claims.Factories;

public static class RoleClaimFactory
{
    public static DomainRoleClaim ToRoleClaimDomain(this PersistenceRoleClaim entity) => DomainRoleClaim.Create(entity.Id, entity.Uid,
        entity.RoleId, entity.ClaimTypeId, entity.ClaimValue, entity.CreatedAt, entity.CreatedOn, entity.UpdatedOn);

    public static PersistenceRoleClaim ToRoleClaimEntity(this DomainRoleClaim domain) => PersistenceRoleClaim.Create(domain.Id, domain.Uid,
        domain.RoleId, domain.ClaimTypeId, domain.ClaimValue, domain.CreatedAt, domain.CreatedOn, domain.UpdatedOn);
}
