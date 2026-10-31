using DomainRefreshToken = App.Core.Domain.Identity.Authentication.RefreshToken;
using PersistenceRefreshToken = App.Core.Persistence.Identity.Authentication.Entities.RefreshToken;

namespace App.Core.Persistence.Identity.Authentication.Factories;

public static class RefreshTokenFactory
{
    public static DomainRefreshToken ToRefreshTokenDomain(this PersistenceRefreshToken entity) =>
        DomainRefreshToken.Create(entity.Id, entity.Uid, entity.UserUid, entity.FamilyUid, entity.TokenHash,
            entity.ExpiresOn, entity.RevokedOn, entity.ReplacedByTokenHash, entity.CreatedOn);

    public static PersistenceRefreshToken ToRefreshTokenEntity(this DomainRefreshToken domain) =>
        PersistenceRefreshToken.Create(domain.Id, domain.Uid, domain.UserUid, domain.FamilyUid, domain.TokenHash,
            domain.ExpiresOn, domain.RevokedOn, domain.ReplacedByTokenHash, domain.CreatedOn);
}
