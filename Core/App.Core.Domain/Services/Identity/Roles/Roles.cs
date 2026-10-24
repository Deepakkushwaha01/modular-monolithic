namespace App.Core.Domain.Identity.Roles
{
    using App.Core.Domain.Configurations.Entity;
    using App.Core.Domain.Configurations.Enums;
    using App.Core.Domain.Identity.Claims;

    public class Role : DomainEntity
    {
        public string Name { get; protected set; } = string.Empty;

        public string Slug { get; protected set; } = string.Empty;

        public string? Description { get; protected set; }

        public bool IsSystemRole { get; protected set; }

        public bool IsActive { get; protected set; }

        public ICollection<UserRole> UserRoles { get; protected set; }
            = new List<UserRole>();

        public ICollection<RoleClaim> RoleClaims { get; protected set; }
            = new List<RoleClaim>();


        #region Update Methods

        public void UpdateName(string name, string slug)
        {
            Name = name;
            Slug = slug;

            MarkModified();
        }

        public void UpdateDescription(string description)
        {
            Description = description;

            MarkModified();
        }

        public void Activate()
        {
            IsActive = true;

            MarkModified();
        }

        public void Deactivate()
        {
            IsActive = false;

            MarkModified();
        }

        #endregion


        #region Create New Role

        public static Role Create(
            string name,
            string slug,
            string? description = null,
            bool isSystemRole = false)
        {
            return new Role
            {
                Uid = Guid.NewGuid(),

                Name = name,
                Slug = slug,
                Description = description,

                IsSystemRole = isSystemRole,
                IsActive = true,

                State = EntityState.Added
            };
        }

        #endregion


        #region DB To Domain

        /// <summary>
        /// Only for database to domain mapping
        /// </summary>
        public static Role Create(
            long id,
            Guid uid,
            string name,
            string slug,
            string? description,
            bool isSystemRole,
            bool isActive,
            DateTime createdOn,
            DateTime? updatedOn,
            IEnumerable<UserRole>? userRoles = null,
            IEnumerable<RoleClaim>? roleClaims = null)
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

                CreatedOn = createdOn,
                UpdatedOn = updatedOn,

                UserRoles = userRoles?.ToList()
                    ?? new List<UserRole>(),

                RoleClaims = roleClaims?.ToList()
                    ?? new List<RoleClaim>()
            };
        }

        #endregion
    }
}
