using Sportradar.Services.Models;

namespace Sportradar.Services
{
    public interface IScoreBoardService
    {
        Task StartGameAsync(string homeTeamCode, string awayTeamCode);
        Task UpdateScoreAsync(UpdateScoreDto updateScoreModel);
        Task FinishGameAsync(string homeTeamCode, string awayTeamCode);
        Task<IEnumerable<GameSummaryDto>> GetSummaryAsync();
    }
}
