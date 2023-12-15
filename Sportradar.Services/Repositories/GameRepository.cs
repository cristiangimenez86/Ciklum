using Microsoft.EntityFrameworkCore;
using Sportradar.Services.DbContexts;
using Sportradar.Services.Entities;
using Sportradar.Services.Models;

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

        public async Task<Game?> GetAsync(string homeTeamCode, string awayTeamCode)
        {
            return await _context
                .Game
                .SingleOrDefaultAsync(e => e.HomeTeamCode.Equals(homeTeamCode)
                                           && e.AwayTeamCode.Equals(awayTeamCode));
        }

        public async Task AddAsync(Game game)
        {
            await _context
                .Game
                .AddAsync(game);
        }

        public async Task UpdateScoreAsync(UpdateScoreDto model)
        {
            var entity = await GetAsync(model.HomeTeamCode, model.AwayTeamCode);
            if (entity == null)
            {
                throw new KeyNotFoundException($"HomeTeamCode: {model.HomeTeamCode} and AwayTeamCode: {model.AwayTeamCode} not found");
            }

            entity.HomeTeamScore = model.HomeTeamScore;
            entity.AwayTeamScore = model.AwayTeamScore;
            entity.TotalScore = entity.HomeTeamScore + model.AwayTeamScore;

            await CommitAsync();
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
