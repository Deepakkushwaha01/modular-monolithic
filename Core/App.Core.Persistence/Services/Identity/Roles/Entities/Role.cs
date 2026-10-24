namespace App.Core.Persistence.Identity.Roles.Entities;

using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Claims.Entities;

public class Role : TrackedEntity
{
    public string Name { get; protected internal set; } = string.Empty;
    public string Slug { get; protected internal set; } = string.Empty;
    public string? Description { get; protected internal set; }
    public bool IsSystemRole { get; protected internal set; }
    public bool IsActive { get; protected internal set; }

    public ICollection<UserRole> UserRoles { get; protected internal set; } = new List<UserRole>();
    public ICollection<RoleClaim> RoleClaims { get; protected internal set; } = new List<RoleClaim>();

    public static Role Create(
        long id,
        Guid uid,
        string name,
        string slug,
        string? description,
        bool isSystemRole,
        bool isActive,
        DateTime createdOn,
        DateTime? updatedOn)
    {
        return new Role
        {
            Id = id,
            Uid = uid,
            Name = name,
            Slug = slug,
            Description = description,
            IsSystemRole = isSystemRole,
            IsActive = isActive,
            UserRoles = new List<UserRole>(),
            RoleClaims = new List<RoleClaim>(),
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
