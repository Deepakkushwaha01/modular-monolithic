namespace App.API.Settings
{
    public class DatabaseConfigurationSettings(IConfiguration _configuration)
    {
        public string DefaultDBConnection => _configuration.GetValue<string>("ConnectionStrings:DefaultDBConnection")!;

    }
}
