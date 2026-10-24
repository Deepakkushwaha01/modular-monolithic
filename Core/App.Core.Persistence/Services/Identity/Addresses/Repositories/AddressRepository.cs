using App.Core.SharedLibrary.Errors;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Addresses.Factories;
using App.Core.Persistence.Identity.Repositories;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.EntityFrameworkCore;
using DomainAddress = App.Core.Domain.Identity.Addresses.Address;
using PersistenceAddress = App.Core.Persistence.Identity.Addresses.Entities.Address;

namespace App.Core.Persistence.Identity.Addresses.Repositories;

public sealed class AddressRepository(IAppDbContext appDbContext)
    : Repository<PersistenceAddress>(appDbContext), IAddressRepository
{
    public async Task<Result<DomainAddress>> GetAddressByUidAsync(Guid uid)
    {
        try
        {
            PersistenceAddress? address = await AllNoTrackedOf().FirstOrDefaultAsync(x => x.Uid == uid);
            return address == null
                ? Result.NotFound<DomainAddress>(PersistenceErrorMessages.EntityNotFound)
                : Result.Ok(address.ToAddressDomain());
        }
        catch (Exception)
        {
            return Result.Failed<DomainAddress>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result<List<DomainAddress>>> GetAddressesAsync()
    {
        try
        {
            List<DomainAddress> addresses = await AllNoTrackedOf()
                .Select(x => x.ToAddressDomain())
                .ToListAsync();

            return Result.Ok(addresses);
        }
        catch (Exception)
        {
            return Result.Failed<List<DomainAddress>>(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public async Task<Result> InsertAsync(DomainAddress entity)
    {
        try
        {
            await _dbSet.AddAsync(entity.ToAddressEntity());
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }

    public void Update(DomainAddress entity)
    {
        AttachOrUpdate(entity.ToAddressEntity(), entity.State.GetState());
    }

    public async Task<Result> DeleteByUidAsync(Guid uid)
    {
        try
        {
            PersistenceAddress? address = await _dbSet.FirstOrDefaultAsync(x => x.Uid == uid);
            if (address == null)
            {
                return Result.NotFound(PersistenceErrorMessages.EntityNotFound);
            }

            Delete(address);
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Failed(PersistenceErrorMessages.DatabaseInternalError);
        }
    }
}
