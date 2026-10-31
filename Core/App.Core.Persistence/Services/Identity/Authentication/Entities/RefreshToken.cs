using App.Core.Persistence.Configurations.Entity;

namespace App.Core.Persistence.Identity.Authentication.Entities;

public sealed class RefreshToken : TrackedEntity
{
    public Guid UserUid { get; private set; }
    public Guid FamilyUid { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresOn { get; private set; }
    public DateTime? RevokedOn { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    public void Revoke(string? replacedByTokenHash = null)
    {
        RevokedOn = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }

    public static RefreshToken Create(long id, Guid uid, Guid userUid, Guid familyUid, string tokenHash,
        DateTime expiresOn, DateTime? revokedOn, string? replacedByTokenHash, DateTime createdOn) => new()
    {
        Id = id,
        Uid = uid,
        UserUid = userUid,
        FamilyUid = familyUid,
        TokenHash = tokenHash,
        ExpiresOn = expiresOn,
        RevokedOn = revokedOn,
        ReplacedByTokenHash = replacedByTokenHash,
        CreatedOn = createdOn
    };
}
