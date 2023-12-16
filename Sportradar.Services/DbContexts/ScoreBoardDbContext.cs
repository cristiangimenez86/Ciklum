using Microsoft.EntityFrameworkCore;
using Sportradar.Services.Data;
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

            modelBuilder.Entity<Team>()
                .HasData(TeamData.GetTeamCollection());

            modelBuilder.Entity<Game>(
                entity =>
                {
                    entity.HasIndex(x => x.HomeTeamCode);
                    entity.HasAlternateKey(x => x.HomeTeamCode);
                    entity.HasIndex(x => x.AwayTeamCode);
                    entity.HasAlternateKey(x => x.AwayTeamCode);
                    entity.HasIndex(x => x.TotalScore);
                    entity.HasIndex(x => x.CreatedDate);
                }
            );
        }

        public DbSet<Game> Game { get; set; } = null!;
        public DbSet<Team> Team { get; set; } = null!;
    }
}