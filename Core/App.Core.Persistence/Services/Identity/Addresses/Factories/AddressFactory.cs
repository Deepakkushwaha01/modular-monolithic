using DomainAddress = App.Core.Domain.Identity.Addresses.Address;
using PersistenceAddress = App.Core.Persistence.Identity.Addresses.Entities.Address;

namespace App.Core.Persistence.Identity.Addresses.Factories;

public static class AddressFactory
{
    public static DomainAddress ToAddressDomain(this PersistenceAddress entity) => DomainAddress.Create(entity.Id, entity.Uid,
        entity.UserId, entity.AddressType, entity.AddressLine1, entity.AddressLine2, entity.City, entity.State,
        entity.ZipCode, entity.Country, entity.IsDefault, entity.CreatedOn, entity.UpdatedOn);

    public static PersistenceAddress ToAddressEntity(this DomainAddress domain) => PersistenceAddress.Create(domain.Id, domain.Uid,
        domain.UserId, domain.AddressType, domain.AddressLine1, domain.AddressLine2, domain.City, domain.StateName,
        domain.ZipCode, domain.Country, domain.IsDefault, domain.CreatedOn, domain.UpdatedOn);
}
