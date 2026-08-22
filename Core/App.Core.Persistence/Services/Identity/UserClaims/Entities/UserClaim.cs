using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Users.Entities;

namespace App.Core.Persistence.Identity.Claims.Entities;

public class UserClaim : TrackedEntity
{
    public long UserId { get; protected internal set; }
    public long ClaimTypeId { get; protected internal set; }
    public string ClaimValue { get; protected internal set; } = "true";

    public User User { get; protected internal set; } = null!;
    public ClaimType ClaimType { get; protected internal set; } = null!;
}
