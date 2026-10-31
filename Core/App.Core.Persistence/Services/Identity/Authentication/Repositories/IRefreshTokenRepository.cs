using App.Core.Persistence.Configurations.Database.Context.Repository;
using PersistenceRefreshToken = App.Core.Persistence.Identity.Authentication.Entities.RefreshToken;

namespace App.Core.Persistence.Identity.Authentication.Repositories;

public interface IRefreshTokenRepository : IRepository<PersistenceRefreshToken>
{
    Task<PersistenceRefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task InsertAsync(PersistenceRefreshToken token);
    Task RevokeFamilyAsync(Guid familyUid);
    Task<bool> TryRevokeAsync(Guid tokenUid, string? replacedByTokenHash);
}
