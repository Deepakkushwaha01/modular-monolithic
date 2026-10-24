using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Claims;

public class UserClaim : DomainEntity
{
    public long UserId { get; protected set; }
    public long ClaimTypeId { get; protected set; }
    public string ClaimValue { get; protected set; } = "true";

    public static UserClaim Create(long userId, long claimTypeId, string claimValue)
    {
        return new UserClaim
        {
            Uid = Guid.NewGuid(), UserId = userId, ClaimTypeId = claimTypeId, ClaimValue = claimValue,
            State = EntityState.Added
        };
    }

    public static UserClaim Create(long id, Guid uid, long userId, long claimTypeId, string claimValue,
        DateTime createdOn, DateTime? updatedOn)
    {
        return new UserClaim
        {
            Id = id, Uid = uid, UserId = userId, ClaimTypeId = claimTypeId, ClaimValue = claimValue,
            CreatedOn = createdOn, UpdatedOn = updatedOn
        };
    }
}
