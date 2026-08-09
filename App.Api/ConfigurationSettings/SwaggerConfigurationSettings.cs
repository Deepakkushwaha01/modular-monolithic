namespace App.API.Settings
{
    public class SwaggerConfigurationSettings(IConfiguration configuration)
    {
        public string Title =>
            configuration.GetValue<string>("Swagger:Title")!;

        public string Version =>
            configuration.GetValue<string>("Swagger:Version")!;

        public string Description =>
            configuration.GetValue<string>("Swagger:Description")!;

        public string RouteTemplate =>
            configuration.GetValue<string>("Swagger:RouteTemplate")!;

        public string JsonEndpointUrl =>
            configuration.GetValue<string>("Swagger:JsonEndpointUrl")!;

        public string RoutePrefix =>
            configuration.GetValue<string>("Swagger:RoutePrefix")!;
    }
}