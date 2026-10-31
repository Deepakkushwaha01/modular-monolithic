namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}
