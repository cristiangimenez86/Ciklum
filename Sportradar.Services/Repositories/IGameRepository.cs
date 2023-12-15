using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Repositories
{
    public interface IGameRepository
    {
        Task<Team?> GetTeam(string code);
        Task AddAsync(Game game);
        Task UpdateScoreAsync(UpdateScoreDto model);
        Task CommitAsync();
    }
}
