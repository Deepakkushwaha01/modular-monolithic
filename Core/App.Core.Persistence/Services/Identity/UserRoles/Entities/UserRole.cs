using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Users.Entities;

namespace App.Core.Persistence.Identity.Roles.Entities;

public class UserRole : TrackedEntity
{
    public Guid UserId { get; protected internal set; }
    public Guid RoleId { get; protected internal set; }
    public DateTime AssignedAt { get; protected internal set; }
    public Guid? AssignedBy { get; protected internal set; }

    public User User { get; protected internal set; } = null!;
    public Role Role { get; protected internal set; } = null!;
    public User? AssignedByUser { get; protected internal set; }

    public static UserRole Create(
        long id,
        Guid uid,
        Guid userId,
        Guid roleId,
        DateTime assignedAt,
        Guid? assignedBy,
        DateTime createdOn,
        DateTime? updatedOn)
    {
        return new UserRole
        {
            Id = id,
            Uid = uid,
            UserId = userId,
            RoleId = roleId,
            AssignedAt = assignedAt,
            AssignedBy = assignedBy,
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
