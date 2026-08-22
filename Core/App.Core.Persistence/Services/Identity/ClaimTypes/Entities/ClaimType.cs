using App.Core.Persistence.Configurations.Entity;

namespace App.Core.Persistence.Identity.Claims.Entities;

public class ClaimType : TrackedEntity
{
    public string Name { get; protected internal set; } = string.Empty;
    public string? Description { get; protected internal set; }
    public ClaimDataType DataType { get; protected internal set; }
    public string Module { get; protected internal set; } = string.Empty;
    public bool IsActive { get; protected internal set; }

    public ICollection<RoleClaim> RoleClaims { get; protected internal set; } = new List<RoleClaim>();
    public ICollection<UserClaim> UserClaims { get; protected internal set; } = new List<UserClaim>();
}
