namespace App.API.Registers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
                WebApplication app = BuildApplication(builder);
                RunApplication(app);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public static WebApplication BuildApplication(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services
                .RegisterSwagger(builder.Configuration)
                .RegisterServices(builder.Configuration, builder.Environment)
                .RegisterDatabase(builder.Configuration);

            return builder.Build();
        }
        
        public static void RunApplication(WebApplication app)
        {
            app.UseStaticFiles();
            app.UseSwaggerDocumentation(app.Configuration);
            app.UseHttpsRedirection();
              app.MapControllers();
            app.Run();
        }
    }
}

