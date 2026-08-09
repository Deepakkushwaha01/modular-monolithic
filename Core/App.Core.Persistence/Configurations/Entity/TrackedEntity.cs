namespace App.Core.Persistence.Configurations.Entity;

public abstract class TrackedEntity
{
    public long Id { get; protected internal set; }
    public Guid Uid { get; protected internal set; }
    public DateTime CreatedOn { get; protected internal set; }
    public DateTime? UpdatedOn { get; protected internal set; }
}