namespace App.Core.Persistence.Configurations.Database.Context
{
    using App.Core.Persistence.Configurations.Database.Context.Extensions.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly bool _loadSeedData;

        public AppDbContext(DbContextOptions<AppDbContext> options, AppDbContextSeedOptions seedOptions) : base(options)
        {
            _loadSeedData = seedOptions.LoadSeedData;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Identity Configurations
            modelBuilder.ConfigureIdentity(_loadSeedData);

        }

        public void HealthCheck()
        {
            Database.OpenConnection();
            Database.CloseConnection();
        }
    }
}
