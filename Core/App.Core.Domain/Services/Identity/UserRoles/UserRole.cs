using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Roles;

public class UserRole : DomainEntity
{
    public Guid UserId { get; protected set; }
    public Guid RoleId { get; protected set; }
    public DateTime AssignedAt { get; protected set; }
    public Guid? AssignedBy { get; protected set; }

    public void UpdateAssignedBy(Guid? assignedBy) { AssignedBy = assignedBy; MarkModified(); }

    public static UserRole Create(Guid userId, Guid roleId, Guid? assignedBy = null)
    {
        return new UserRole
        {
            Uid = Guid.NewGuid(),
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow,
            AssignedBy = assignedBy,
            State = EntityState.Added
        };
    }

    public static UserRole Create(long id, Guid uid, Guid userId, Guid roleId, DateTime assignedAt, Guid? assignedBy,
        DateTime createdOn, DateTime? updatedOn)
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
