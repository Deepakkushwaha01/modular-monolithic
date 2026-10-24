namespace App.Core.Domain.Configurations.Enums
{
    /// <summary>
    /// Describes the state of an entity.
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// The entity is being tracked by the context and exists in the database,
        /// and its property values have not changed from the values in the database.
        /// </summary>
        Unchanged = 2,

        /// <summary>
        /// The entity is being tracked by the context but does not yet exist in the database.
        /// </summary>
        Added = 4,

        /// <summary>
        /// The entity is being tracked by the context and exists in the database, but has
        /// been marked for deletion from the database the next time SaveChanges is called.
        /// </summary>
        Deleted = 8,

        /// <summary>
        /// The entity is being tracked by the context and exists in the database, and some
        /// or all of its property values have been modified.
        /// </summary>
        Modified = 16
    }
}
