using App.API.Settings;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, IConfiguration config)
    {
        SwaggerConfigurationSettings swagger = new SwaggerConfigurationSettings(config);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            foreach (SwaggerDocumentSettings document in swagger.Documents)
            {
                options.SwaggerDoc(document.Name, new OpenApiInfo
                {
                    Title = document.Title,
                    Version = document.Version,
                    Description = document.Description
                });
            }

            options.DocInclusionPredicate((documentName, apiDescription) =>
            {
                SwaggerDocumentSettings? document = swagger.Documents
                    .SingleOrDefault(item => string.Equals(item.Name, documentName, StringComparison.OrdinalIgnoreCase));
                SwaggerModuleAttribute? module = (apiDescription.ActionDescriptor as ControllerActionDescriptor)?
                    .ControllerTypeInfo.GetCustomAttribute<SwaggerModuleAttribute>();

                return document is not null
                    && module is not null
                    && string.Equals(document.Module, module.Name, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(document.Version, apiDescription.GroupName, StringComparison.OrdinalIgnoreCase);
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
        SwaggerConfigurationSettings swagger = new SwaggerConfigurationSettings(config);

        app.UseSwagger(c =>
        {
            c.RouteTemplate = swagger.RouteTemplate;
        });

        app.UseSwaggerUI(c =>
        {
            c.DisplayRequestDuration();

            foreach (SwaggerDocumentSettings document in swagger.Documents)
            {
                c.SwaggerEndpoint($"/{swagger.RoutePrefix}/{document.Name}/swagger.json", document.Title);
            }

            c.InjectJavascript("/swagger/select-api-label.js");
            c.InjectStylesheet("/swagger/select-api-label.css");
            c.RoutePrefix = swagger.RoutePrefix;
        });

        return app;
    }
}
