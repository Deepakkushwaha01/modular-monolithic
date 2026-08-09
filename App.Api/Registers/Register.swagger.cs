using App.API.Settings;
using Microsoft.OpenApi.Models;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, IConfiguration config)
    {
        SwaggerConfigurationSettings swagger = new SwaggerConfigurationSettings(config);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = swagger.Title,
                Version = swagger.Version,
                Description = swagger.Description
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter JWT token. Example: Bearer eyJhbGciOiJIUzI1NiIs...",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration config)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = config["Swagger:RouteTemplate"];
        });

        app.UseSwaggerUI(c =>
        {
            c.DisplayRequestDuration();
            c.SwaggerEndpoint(
                config["Swagger:JsonEndpointUrl"],
                config["Swagger:Title"]);

            c.RoutePrefix = config["Swagger:RoutePrefix"];
        });

        return app;
    }
}