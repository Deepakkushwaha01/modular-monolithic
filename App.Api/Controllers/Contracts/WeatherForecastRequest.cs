using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Contracts;

public sealed class WeatherForecastRequest
{
    [Required(ErrorMessage = CommonErrorMessages.NameRequired)]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = CommonErrorMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = CommonErrorMessages.EmailAddressInvalid)]
    public string Email { get; init; } = string.Empty;
}
