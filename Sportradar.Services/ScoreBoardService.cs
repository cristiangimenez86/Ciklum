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
            if (string.IsNullOrEmpty(homeTeamCode) || string.IsNullOrEmpty(awayTeamCode))
            {
                throw new ArgumentException($"Parameter {nameof(homeTeamCode)} or {nameof(awayTeamCode)} cannot be null or empty");
            }

            var homeTeam = await _gameRepository.GetTeam(homeTeamCode);
            var awayTeam = await _gameRepository.GetTeam(awayTeamCode);

            if (homeTeam == null || awayTeam == null)
            {
                throw new InvalidOperationException($"Teams not found: {homeTeam} or {awayTeam}.");
            }

            var game = new Game
            {
                HomeTeamCode = homeTeam.TeamCode,
                HomeTeamName = homeTeam.TeamName,
                AwayTeamCode = awayTeam.TeamCode,
                AwayTeamName = awayTeam.TeamName,
            };

            await _gameRepository.AddAsync(game);
            await _gameRepository.CommitAsync();
        }
    }
}
