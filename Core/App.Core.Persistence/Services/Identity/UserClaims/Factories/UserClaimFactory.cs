using DomainUserClaim = App.Core.Domain.Identity.Claims.UserClaim;
using PersistenceUserClaim = App.Core.Persistence.Identity.Claims.Entities.UserClaim;

namespace App.Core.Persistence.Identity.Claims.Factories;

public static class UserClaimFactory
{
    public static DomainUserClaim ToUserClaimDomain(this PersistenceUserClaim entity) => DomainUserClaim.Create(entity.Id, entity.Uid,
        entity.UserId, entity.ClaimTypeId, entity.ClaimValue, entity.CreatedOn, entity.UpdatedOn);

    public static PersistenceUserClaim ToUserClaimEntity(this DomainUserClaim domain) => PersistenceUserClaim.Create(domain.Id, domain.Uid,
        domain.UserId, domain.ClaimTypeId, domain.ClaimValue, domain.CreatedOn, domain.UpdatedOn);
}
