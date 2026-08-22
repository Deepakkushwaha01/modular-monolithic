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
}
