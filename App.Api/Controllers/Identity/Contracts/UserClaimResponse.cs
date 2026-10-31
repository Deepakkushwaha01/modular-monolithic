using App.Core.Domain.Identity.Claims;


namespace App.API.Controllers.Identity.Contracts;

public sealed record UserClaimResponse(Guid Uid, Guid UserId, Guid ClaimTypeId, string ClaimValue)
{
    public static UserClaimResponse From(UserClaim item) => new(item.Uid, item.UserId, item.ClaimTypeId, item.ClaimValue);
}
