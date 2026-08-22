using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Users.Entities;

namespace App.Core.Persistence.Identity.Roles.Entities;

public class UserRole : TrackedEntity
{
    public long UserId { get; protected internal set; }
    public long RoleId { get; protected internal set; }
    public DateTime AssignedAt { get; protected internal set; }
    public long? AssignedBy { get; protected internal set; }

    public User User { get; protected internal set; } = null!;
    public Role Role { get; protected internal set; } = null!;
    public User? AssignedByUser { get; protected internal set; }
}
