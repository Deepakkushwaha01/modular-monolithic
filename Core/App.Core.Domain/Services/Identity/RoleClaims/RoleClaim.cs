using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Claims;

public class RoleClaim : DomainEntity
{
    public long RoleId { get; protected set; }
    public long ClaimTypeId { get; protected set; }
    public string ClaimValue { get; protected set; } = "true";
    public DateTime CreatedAt { get; protected set; }

    public void Update(string claimValue) { ClaimValue = claimValue; MarkModified(); }

    public static RoleClaim Create(long roleId, long claimTypeId, string claimValue)
    {
        return new RoleClaim
        {
            Uid = Guid.NewGuid(),
            RoleId = roleId,
            ClaimTypeId = claimTypeId,
            ClaimValue = claimValue,
            CreatedAt = DateTime.UtcNow,
            State = EntityState.Added
        };
    }

    public static RoleClaim Create(long id, Guid uid, long roleId, long claimTypeId, string claimValue,
        DateTime createdAt, DateTime createdOn, DateTime? updatedOn)
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
