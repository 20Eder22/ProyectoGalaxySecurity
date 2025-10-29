using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infraestructure
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            const string connectionString = "Host=localhost;Port=1500;Database=security_db;Username=admin;Password=Password2025";
            
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            optionsBuilder.UseNpgsql(connectionString,
                opt => opt.MigrationsHistoryTable("__EFMigrationHistory"));
            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
