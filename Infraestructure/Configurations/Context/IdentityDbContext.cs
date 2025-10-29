using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<UserExtension>(options)
    {

        public DbSet<Reclamo> Reclamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reclamo>()
                .Property(e => e.Fecha)
                .HasColumnType("timestamp without time zone");
                    }

    }
}
