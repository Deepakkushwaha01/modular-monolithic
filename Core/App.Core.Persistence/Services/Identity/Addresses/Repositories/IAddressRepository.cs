using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.SharedLibrary.Patterns.Result;
using DomainAddress = App.Core.Domain.Identity.Addresses.Address;
using PersistenceAddress = App.Core.Persistence.Identity.Addresses.Entities.Address;

namespace App.Core.Persistence.Identity.Addresses.Repositories;

public interface IAddressRepository : IRepository<PersistenceAddress>
{
    Task<Result<DomainAddress>> GetAddressByUidAsync(Guid uid);

    Task<Result<List<DomainAddress>>> GetAddressesAsync();

    Task<Result> InsertAsync(DomainAddress entity);

    void Update(DomainAddress entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}