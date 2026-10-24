using App.Core.Domain.Configurations.Entity;
using App.Core.Domain.Configurations.Enums;

namespace App.Core.Domain.Identity.Roles;

public class UserRole : DomainEntity
{
    public long UserId { get; protected set; }
    public long RoleId { get; protected set; }
    public DateTime AssignedAt { get; protected set; }
    public long? AssignedBy { get; protected set; }

    public static UserRole Create(long userId, long roleId, long? assignedBy = null)
    {
        return new UserRole
        {
            Uid = Guid.NewGuid(), UserId = userId, RoleId = roleId, AssignedAt = DateTime.UtcNow,
            AssignedBy = assignedBy, State = EntityState.Added
        };
    }

    public static UserRole Create(long id, Guid uid, long userId, long roleId, DateTime assignedAt, long? assignedBy,
        DateTime createdOn, DateTime? updatedOn)
    {
        return new UserRole
        {
            Id = id, Uid = uid, UserId = userId, RoleId = roleId, AssignedAt = assignedAt, AssignedBy = assignedBy,
            CreatedOn = createdOn, UpdatedOn = updatedOn
        };
    }
}
