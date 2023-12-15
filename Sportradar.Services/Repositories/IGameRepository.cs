using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Repositories
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task UpdateScoreAsync(UpdateScoreDto model);
        Task DeleteAsync(string homeTeamCode, string awayTeamCode);
        Task CommitAsync();
        Task<Team?> GetTeamAsync(string code);
    }
}
