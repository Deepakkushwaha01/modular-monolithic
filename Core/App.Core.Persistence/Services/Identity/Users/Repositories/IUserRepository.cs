using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.SharedLibrary.Patterns.Result;
using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

namespace App.Core.Persistence.Identity.Users.Repositories;

public interface IUserRepository : IRepository<PersistenceUser>
{
    Task<Result<DomainUser>> GetUserByUidAsync(Guid uid);

    Task<Result<List<DomainUser>>> GetUsersAsync();

    Task<bool> EmailExistsAsync(string email);

    Task<Result> InsertAsync(DomainUser entity);

    void Update(DomainUser entity);

    Task<Result> DeleteByUidAsync(Guid uid);
}
