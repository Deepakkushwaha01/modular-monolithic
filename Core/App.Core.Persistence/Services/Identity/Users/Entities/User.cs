using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Addresses.Entities;
using App.Core.Persistence.Identity.Claims.Entities;
using App.Core.Persistence.Identity.Roles.Entities;

namespace App.Core.Persistence.Identity.Users.Entities
{
    public class User : TrackedEntity
    {
        public ICollection<Address> Addresses { get; protected internal set; } = new List<Address>();

        public ICollection<UserRole> UserRoles { get; protected internal set; } = new List<UserRole>();

        public ICollection<UserClaim> UserClaims { get; protected internal set; } = new List<UserClaim>();

        public string Email { get; protected internal set; } = string.Empty;

        public string Password { get; protected internal set; } = string.Empty;

        public string? FirstName { get; protected internal set; }

        public string? LastName { get; protected internal set; }

        public string? Phone { get; protected internal set; }

        public bool IsPhoneVerified { get; protected internal set; }

        public bool IsActive { get; protected internal set; }

        public bool IsBlocked { get; protected internal set; }

        public bool IsEmailVerified { get; protected internal set; }

        public DateTime? LastLoginDate { get; protected internal set; }

        public string? ProfilePhoto { get; protected internal set; }

        public static User Create(
            long id,
            Guid uid,
            string email,
            string password,
            string? firstName,
            string? lastName,
            string? phone,
            bool isPhoneVerified,
            bool isActive,
            bool isEmailVerified,
            DateTime? lastLoginDate,
            string? profilePhoto,
            DateTime createdOn,
            DateTime? updatedOn)
        {
            return new User
            {
                Id = id,
                Uid = uid,
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                IsPhoneVerified = isPhoneVerified,
                IsActive = isActive,
                IsBlocked = false,
                IsEmailVerified = isEmailVerified,
                LastLoginDate = lastLoginDate,
                ProfilePhoto = profilePhoto,
                CreatedOn = createdOn,
                UpdatedOn = updatedOn
            };
        }
    }
}
