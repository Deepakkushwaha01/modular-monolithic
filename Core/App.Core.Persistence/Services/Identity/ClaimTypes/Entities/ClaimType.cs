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

    public static ClaimType Create(
        long id,
        Guid uid,
        string name,
        string? description,
        ClaimDataType dataType,
        string module,
        bool isActive,
        DateTime createdOn,
        DateTime? updatedOn)
    {
        return new ClaimType
        {
            Id = id,
            Uid = uid,
            Name = name,
            Description = description,
            DataType = dataType,
            Module = module,
            IsActive = isActive,
            RoleClaims = new List<RoleClaim>(),
            UserClaims = new List<UserClaim>(),
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
