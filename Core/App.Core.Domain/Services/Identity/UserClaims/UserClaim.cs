using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Claims;

public class UserClaim : DomainEntity
{
    public Guid UserId { get; protected set; }
    public Guid ClaimTypeId { get; protected set; }
    public string ClaimValue { get; protected set; } = "true";

    public void Update(string claimValue) { ClaimValue = claimValue; MarkModified(); }

    public static UserClaim Create(Guid userId, Guid claimTypeId, string claimValue)
    {
        return new UserClaim
        {
            Uid = Guid.NewGuid(),
            UserId = userId,
            ClaimTypeId = claimTypeId,
            ClaimValue = claimValue,
            State = EntityState.Added
        };
    }

    public static UserClaim Create(long id, Guid uid, Guid userId, Guid claimTypeId, string claimValue,
        DateTime createdOn, DateTime? updatedOn)
    {
        return new UserClaim
        {
            Id = id,
            Uid = uid,
            UserId = userId,
            ClaimTypeId = claimTypeId,
            ClaimValue = claimValue,
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
