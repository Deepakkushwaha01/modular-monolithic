namespace App.Core.Persistence.Configurations.Database.Context
{
    using App.Core.Persistence.Configurations.Database.Context.Extensions.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Identity Configurations
            modelBuilder.ConfigureIdentity();

        }

        public void HealthCheck()
        {
            Database.OpenConnection();
            Database.CloseConnection();
        }
    }
}