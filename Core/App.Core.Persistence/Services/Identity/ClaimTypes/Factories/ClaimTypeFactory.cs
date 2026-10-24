using DomainClaimDataType = App.Core.Domain.Identity.Claims.ClaimDataType;
using DomainClaimType = App.Core.Domain.Identity.Claims.ClaimType;
using PersistenceClaimDataType = App.Core.Persistence.Identity.Claims.Entities.ClaimDataType;
using PersistenceClaimType = App.Core.Persistence.Identity.Claims.Entities.ClaimType;

namespace App.Core.Persistence.Identity.Claims.Factories;

public static class ClaimTypeFactory
{
    public static DomainClaimType ToClaimTypeDomain(this PersistenceClaimType entity) => DomainClaimType.Create(entity.Id, entity.Uid,
        entity.Name, entity.Description, (DomainClaimDataType)entity.DataType, entity.Module, entity.IsActive,
        entity.CreatedOn, entity.UpdatedOn);

    public static PersistenceClaimType ToClaimTypeEntity(this DomainClaimType domain) => PersistenceClaimType.Create(domain.Id, domain.Uid,
        domain.Name, domain.Description, (PersistenceClaimDataType)domain.DataType, domain.Module, domain.IsActive,
        domain.CreatedOn, domain.UpdatedOn);
}
