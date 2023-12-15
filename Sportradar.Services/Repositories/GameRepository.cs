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
    }
}
