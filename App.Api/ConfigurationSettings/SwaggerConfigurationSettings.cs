namespace App.API.Settings
{
    using App.Core.SharedLibrary.Errors;

    public class SwaggerConfigurationSettings(IConfiguration configuration)
    {
        public string RouteTemplate =>
            configuration.GetValue<string>("Swagger:RouteTemplate")!;

        public string RoutePrefix =>
            configuration.GetValue<string>("Swagger:RoutePrefix")!;

        public IReadOnlyList<SwaggerDocumentSettings> Documents =>
            configuration.GetSection("Swagger:Documents").Get<List<SwaggerDocumentSettings>>()
            ?? throw new InvalidOperationException(CommonErrorMessages.SwaggerDocumentRequired);
    }

    public sealed class SwaggerDocumentSettings
    {
        public required string Name { get; init; }
        public required string Module { get; init; }
        public required string Title { get; init; }
        public required string Version { get; init; }
        public required string Description { get; init; }
    }
}
