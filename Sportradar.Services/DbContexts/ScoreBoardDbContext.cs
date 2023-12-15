using Microsoft.EntityFrameworkCore;

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
            
        }
    }
}