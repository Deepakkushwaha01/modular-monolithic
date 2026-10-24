namespace App.Core.Domain.Configurations.Entity
{
    using System;
    using App.Core.Domain.Configurations.Enums;

    public abstract class DomainEntity
    {
        public long Id { get; protected set; }

        public Guid Uid { get; protected set; }

        public DateTime CreatedOn { get; protected set; }

        public DateTime? UpdatedOn { get; protected set; }

        public string CreatedBy { get; protected set; } = string.Empty;

        public string? UpdatedBy { get; protected set; } = string.Empty;

        public EntityState State { get; protected set; }

        public bool IsStateChanged => State != EntityState.Unchanged;

        protected DomainEntity()
        {
        }

        protected DomainEntity(long id, Guid uid, DateTime createdOn, DateTime? updatedOn, EntityState state, string createdBy, string? updatedBy)
        {
            Id = id;
            Uid = uid;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            State = state;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
        }

        public void MarkModified()
        {
            State = EntityState.Modified;
        }

        public void MarkUpdated(DateTime updatedOn)
        {
            UpdatedOn = updatedOn;

            MarkModified();
        }
    }
}
