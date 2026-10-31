using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.Repository;
using App.Core.Persistence.Identity.Authentication.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Persistence.Identity.Authentication.Repositories;

public sealed class RefreshTokenRepository(IAppDbContext appDbContext)
    : Repository<RefreshToken>(appDbContext), IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash) =>
        _dbSet.SingleOrDefaultAsync(token => token.TokenHash == tokenHash);

    public async Task InsertAsync(RefreshToken token) => await _dbSet.AddAsync(token);

    public async Task RevokeFamilyAsync(Guid familyUid)
    {
        DateTime now = DateTime.UtcNow;
        await _dbSet.Where(token => token.FamilyUid == familyUid && token.RevokedOn == null)
            .ExecuteUpdateAsync(update => update.SetProperty(token => token.RevokedOn, now));
    }

    public async Task<bool> TryRevokeAsync(Guid tokenUid, string? replacedByTokenHash)
    {
        DateTime now = DateTime.UtcNow;
        int updated = await _dbSet.Where(token => token.Uid == tokenUid && token.RevokedOn == null)
            .ExecuteUpdateAsync(update => update
                .SetProperty(token => token.RevokedOn, now)
                .SetProperty(token => token.ReplacedByTokenHash, replacedByTokenHash));
        return updated == 1;
    }
}
