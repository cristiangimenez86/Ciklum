using Sportradar.Services.DbContexts;

namespace Sportradar.Services.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ScoreBoardDbContext _context;

        public GameRepository(ScoreBoardDbContext context)
        {
            _context = context;
        }
    }
}
