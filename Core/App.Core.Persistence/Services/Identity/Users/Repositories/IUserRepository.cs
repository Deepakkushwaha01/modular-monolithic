using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.SharedLibrary.Patterns.Result;
using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

namespace App.Core.Persistence.Identity.Users.Repositories;

public interface IUserRepository : IRepository<PersistenceUser>
{
    Task<Result<DomainUser>> GetUserByUidAsync(
        Guid uid,
        bool includeAddresses = false,
        bool includeRoles = false,
        bool includeClaims = false);

    Task<Result<List<DomainUser>>> GetUsersAsync(
        bool includeAddresses = false,
        bool includeRoles = false,
        bool includeClaims = false);

    Task<bool> EmailExistsAsync(string email);

    Task<Result> InsertAsync(DomainUser entity);

    void Update(DomainUser entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}
