using App.Core.Domain.Identity.Addresses;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using System.Text.Json.Serialization;

namespace App.API.Controllers.Identity.Contracts;

public sealed record AddressResponse(Guid Uid, string AddressType, string AddressLine1, string? AddressLine2,
    string City, string State, string ZipCode, string Country, bool IsDefault)
{
    public static AddressResponse From(Address address) => new(address.Uid, address.AddressType, address.AddressLine1,
        address.AddressLine2, address.City, address.StateName, address.ZipCode, address.Country, address.IsDefault);
}
