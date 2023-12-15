using Microsoft.EntityFrameworkCore;
using Sportradar.Services.Entities;

namespace Sportradar.Services.DbContexts
{
    public class ScoreBoardDbContext : DbContext
    {
        public ScoreBoardDbContext(DbContextOptions options) : base(options)
        {

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) { throw new ArgumentNullException(nameof(modelBuilder)); }

            modelBuilder.Entity<Game>(
                entity =>
                {
                    entity.HasIndex(x => x.HomeTeamCode).IsUnique();
                    entity.HasIndex(x => x.AwayTeamCode).IsUnique();
                    entity.HasIndex(x => x.TotalScore);
                    entity.HasIndex(x => x.CreatedDate);
                }
            );
        }

        public DbSet<Game> Game { get; set; }
    }
}