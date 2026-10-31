namespace App.Core.Domain.Identity.Users
{
    using App.Core.Domain.Configurations.Entity;
    using App.Core.Domain.Configurations.Enums;
    using App.Core.Domain.Identity.Addresses;
    using App.Core.Domain.Identity.Claims;
    using App.Core.Domain.Identity.Roles;
    using App.Core.SharedLibrary.Enums;

    public class User : DomainEntity
    {
        public string Email { get; protected set; } = string.Empty;

        public string Password { get; protected set; } = string.Empty;

        public string? FirstName { get; protected set; }

        public string? LastName { get; protected set; }

        public string Phone { get; protected set; } = string.Empty;

        public bool IsPhoneVerified { get; protected set; }

        public bool IsEmailVerified { get; protected set; }

        public bool IsActive { get; protected set; }

        public bool IsBlocked { get; protected set; }

        public DateTime? LastLoginDate { get; protected set; }

        public Portal Portal { get; protected set; }

        public string? ProfilePhoto { get; protected set; }

        public string AccessToken { get; protected set; } = string.Empty;

        public string RefreshToken { get; protected set; } = string.Empty;

        public DateTimeOffset AccessTokenExpiresAt { get; protected set; }

        public DateTimeOffset RefreshTokenExpiresAt { get; protected set; }

        public List<string> Roles { get; protected internal set; } = new List<string>();

        public List<string> Claims { get; protected internal set; } = new List<string>();

        public ICollection<Address> Addresses { get; protected set; }
            = new List<Address>();

        public ICollection<UserRole> UserRoles { get; protected set; }
            = new List<UserRole>();

        public ICollection<UserClaim> UserClaims { get; protected set; }
            = new List<UserClaim>();


        #region Update Methods

        public void UpdateProfile(
            string? firstName,
            string? lastName,
            string phone)
        {
            if (!string.Equals(Phone, phone, StringComparison.Ordinal))
                IsPhoneVerified = false;

            FirstName = firstName;
            LastName = lastName;
            Phone = phone;

            MarkModified();
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            IsEmailVerified = false;

            MarkModified();
        }

        public void UpdatePassword(string password)
        {
            Password = password;

            MarkModified();
        }

        public void UpdatePhone(string phone)
        {
            if (!string.Equals(Phone, phone, StringComparison.Ordinal))
                IsPhoneVerified = false;

            Phone = phone;

            MarkModified();
        }

        public void VerifyEmail()
        {
            IsEmailVerified = true;

            MarkModified();
        }

        public void VerifyPhone()
        {
            IsPhoneVerified = true;

            MarkModified();
        }

        public void Activate()
        {
            IsActive = true;
            IsBlocked = false;

            MarkModified();
        }

        public void Deactivate()
        {
            IsActive = false;
            IsBlocked = true;

            MarkModified();
        }

        public void UpdateLastLogin()
        {
            LastLoginDate = DateTime.UtcNow;

            MarkModified();
        }

        public void AddRoles(IEnumerable<string> roles)
        {
            Roles = roles.ToList();
        }

        public void AddClaims(IEnumerable<string> claims)
        {
            Claims = claims.ToList();
        }

        public void UpdateAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        public void UpdateTokenPair(string accessToken, string refreshToken, DateTimeOffset accessTokenExpiresAt,
            DateTimeOffset refreshTokenExpiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        #endregion


        #region Create New User

        public static User Create(
            string email,
            string password,
            string phone,
            Portal portal,
            string? firstName = null,
            string? lastName = null)
        {
            return new User
            {
                Uid = Guid.NewGuid(),

                Email = email,
                Password = password,
                Portal = portal,

                FirstName = firstName,
                LastName = lastName,
                Phone = phone,

                IsPhoneVerified = false,
                IsEmailVerified = false,

                IsActive = true,
                IsBlocked = false,

                LastLoginDate = null,
                ProfilePhoto = null,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = email,

                State = EntityState.Added
            };
        }

        #endregion


        #region DB To Domain

        /// <summary>
        /// Only for database to domain mapping
        /// </summary>
        public static User Create(
            long id,
            Guid uid,
            string email,
            string password,
            string? firstName,
            string? lastName,
            string phone,
            Portal portal,
            bool isPhoneVerified,
            bool isActive,
            bool isBlocked,
            bool isEmailVerified,
            DateTime? lastLoginDate,
            string? profilePhoto,
            DateTime createdOn,
            DateTime? updatedOn,
            string createdBy,
            string? updatedBy,
            IEnumerable<Address>? addresses = null,
            IEnumerable<UserRole>? userRoles = null,
            IEnumerable<UserClaim>? userClaims = null)
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
                Portal = portal,

                IsPhoneVerified = isPhoneVerified,
                IsActive = isActive,
                IsBlocked = isBlocked,
                IsEmailVerified = isEmailVerified,

                LastLoginDate = lastLoginDate,
                ProfilePhoto = profilePhoto,

                CreatedOn = createdOn,
                UpdatedOn = updatedOn,

                CreatedBy = createdBy,
                UpdatedBy = updatedBy,

                Addresses = addresses?.ToList()
                    ?? new List<Address>(),

                UserRoles = userRoles?.ToList()
                    ?? new List<UserRole>(),

                UserClaims = userClaims?.ToList()
                    ?? new List<UserClaim>()
            };
        }

        #endregion
    }
}
