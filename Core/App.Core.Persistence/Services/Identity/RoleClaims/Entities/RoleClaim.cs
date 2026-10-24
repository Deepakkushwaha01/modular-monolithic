using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Roles.Entities;

namespace App.Core.Persistence.Identity.Claims.Entities;

public class RoleClaim : TrackedEntity
{
    public long RoleId { get; protected internal set; }
    public long ClaimTypeId { get; protected internal set; }
    public string ClaimValue { get; protected internal set; } = "true";
    public DateTime CreatedAt { get; protected internal set; }

    public Role Role { get; protected internal set; } = null!;
    public ClaimType ClaimType { get; protected internal set; } = null!;

    public static RoleClaim Create(
        long id,
        Guid uid,
        long roleId,
        long claimTypeId,
        string claimValue,
        DateTime createdAt,
        DateTime createdOn,
        DateTime? updatedOn)
    {
        return new RoleClaim
        {
            Id = id,
            Uid = uid,
            RoleId = roleId,
            ClaimTypeId = claimTypeId,
            ClaimValue = claimValue,
            CreatedAt = createdAt,
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
