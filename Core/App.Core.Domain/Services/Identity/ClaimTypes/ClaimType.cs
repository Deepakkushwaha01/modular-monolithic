using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Claims;

public class ClaimType : DomainEntity
{
    public string Name { get; protected set; } = string.Empty;
    public string? Description { get; protected set; }
    public ClaimDataType DataType { get; protected set; }
    public string Module { get; protected set; } = string.Empty;
    public bool IsActive { get; protected set; }
    public ICollection<RoleClaim> RoleClaims { get; protected set; } = new List<RoleClaim>();
    public ICollection<UserClaim> UserClaims { get; protected set; } = new List<UserClaim>();

    public void Update(string name, string? description, ClaimDataType dataType, string module, bool isActive)
    {
        Name = name; Description = description; DataType = dataType; Module = module; IsActive = isActive;
        MarkModified();
    }

    public static ClaimType Create(string name, string? description, ClaimDataType dataType, string module)
    {
        return new ClaimType
        {
            Uid = Guid.NewGuid(),
            Name = name,
            Description = description,
            DataType = dataType,
            Module = module,
            IsActive = true,
            State = EntityState.Added
        };
    }

    public static ClaimType Create(long id, Guid uid, string name, string? description, ClaimDataType dataType,
        string module, bool isActive, DateTime createdOn, DateTime? updatedOn,
        IEnumerable<RoleClaim>? roleClaims = null, IEnumerable<UserClaim>? userClaims = null)
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
            CreatedOn = createdOn,
            UpdatedOn = updatedOn,
            RoleClaims = roleClaims?.ToList() ?? new List<RoleClaim>(),
            UserClaims = userClaims?.ToList() ?? new List<UserClaim>()
        };
    }
}
