namespace App.Core.Domain.Identity.Addresses
{
    using App.Core.Domain.Configurations.Entity;
    using App.Core.Domain.Configurations.Enums;

    public class Address : DomainEntity
    {
        public long UserId { get; protected set; }
        public string AddressType { get; protected set; } = string.Empty;
        public string AddressLine1 { get; protected set; } = string.Empty;
        public string? AddressLine2 { get; protected set; }
        public string City { get; protected set; } = string.Empty;
        public string StateName { get; protected set; } = string.Empty;
        public string ZipCode { get; protected set; } = string.Empty;
        public string Country { get; protected set; } = string.Empty;
        public bool IsDefault { get; protected set; }

        public static Address Create(
            long userId,
            string addressType,
            string addressLine1,
            string? addressLine2,
            string city,
            string state,
            string zipCode,
            string country,
            bool isDefault)
        {
            return new Address
            {
                Uid = Guid.NewGuid(),
                UserId = userId,
                AddressType = addressType,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                StateName = state,
                ZipCode = zipCode,
                Country = country,
                IsDefault = isDefault,
                State = EntityState.Added
            };
        }

        // DB to domain mapping
        public static Address Create(
            long id,
            Guid uid,
            long userId,
            string addressType,
            string addressLine1,
            string? addressLine2,
            string city,
            string state,
            string zipCode,
            string country,
            bool isDefault,
            DateTime createdOn,
            DateTime? updatedOn)
        {
            return new Address
            {
                Id = id,
                Uid = uid,
                UserId = userId,
                AddressType = addressType,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                StateName = state,
                ZipCode = zipCode,
                Country = country,
                IsDefault = isDefault,
                CreatedOn = createdOn,
                UpdatedOn = updatedOn
            };
        }
    }
}
