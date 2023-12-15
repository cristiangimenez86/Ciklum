using Microsoft.EntityFrameworkCore;
using Sportradar.Services.DbContexts;
using Sportradar.Services.Entities;

namespace Sportradar.Services.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ScoreBoardDbContext _context;

        public GameRepository(ScoreBoardDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task AddAsync(Game game)
        {
            await _context
                .Game
                .AddAsync(game);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Team?> GetTeam(string code)
        {
            return await _context
                .Team
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TeamCode.Equals(code));
        }
    }
}
