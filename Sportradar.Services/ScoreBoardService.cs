using Microsoft.Extensions.Logging;
using Sportradar.Services.Entities;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;

namespace Sportradar.Services
{
    public class ScoreBoardService : IScoreBoardService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IValidator _validator;
        private readonly ILogger<ScoreBoardService> _logger;

        public ScoreBoardService(
            IGameRepository gameRepository, 
            IValidator validator, 
            ILogger<ScoreBoardService> logger)
        {
            _gameRepository = gameRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task StartGame(string homeTeamCode, string awayTeamCode)
        {
            var game = new Game
            {
                HomeTeamCode = homeTeamCode,
                HomeTeamName = "",
                AwayTeamCode = awayTeamCode,
                AwayTeamName = "",
            };

            await _gameRepository.AddAsync(game);
            await _gameRepository.CommitAsync();
        }
    }
}
