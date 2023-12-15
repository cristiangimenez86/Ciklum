using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAsync();
        Task AddAsync(Game game);
        Task UpdateScoreAsync(UpdateScoreDto model);
        Task DeleteAsync(string homeTeamCode, string awayTeamCode);
        Task CommitAsync();
        Task<Team?> GetTeamAsync(string code);
    }
}
