namespace App.Core.Domain.Identity.Authentication;

public sealed class RefreshToken
{
    public long Id { get; private set; }
    public Guid Uid { get; private set; }
    public Guid UserUid { get; private set; }
    public Guid FamilyUid { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresOn { get; private set; }
    public DateTime? RevokedOn { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public static RefreshToken Create(Guid userUid, Guid familyUid, string tokenHash, DateTime expiresOn) => new()
    {
        Uid = Guid.NewGuid(),
        UserUid = userUid,
        FamilyUid = familyUid,
        TokenHash = tokenHash,
        ExpiresOn = expiresOn,
        CreatedOn = DateTime.UtcNow
    };

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

    public void Revoke(string? replacedByTokenHash = null)
    {
        RevokedOn = DateTime.UtcNow;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}
