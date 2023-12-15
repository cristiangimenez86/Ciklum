using Sportradar.Services.Entities;

namespace Sportradar.Services.Repositories
{
    public interface IGameRepository
    {
        Task AddAsync(Game game);
        Task CommitAsync();
    }
}
