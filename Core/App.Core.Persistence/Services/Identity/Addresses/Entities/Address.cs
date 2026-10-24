using App.Core.Persistence.Configurations.Entity;
using App.Core.Persistence.Identity.Users.Entities;

namespace App.Core.Persistence.Identity.Addresses.Entities;

public class Address : TrackedEntity
{
    public long UserId { get; protected internal set; }
    public string AddressType { get; protected internal set; } = string.Empty;
    public string AddressLine1 { get; protected internal set; } = string.Empty;
    public string? AddressLine2 { get; protected internal set; }
    public string City { get; protected internal set; } = string.Empty;
    public string State { get; protected internal set; } = string.Empty;
    public string ZipCode { get; protected internal set; } = string.Empty;
    public string Country { get; protected internal set; } = string.Empty;
    public bool IsDefault { get; protected internal set; }

    public User User { get; protected internal set; } = null!;

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
            State = state,
            ZipCode = zipCode,
            Country = country,
            IsDefault = isDefault,
            CreatedOn = createdOn,
            UpdatedOn = updatedOn
        };
    }
}
