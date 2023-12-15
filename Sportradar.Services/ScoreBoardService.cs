using Microsoft.Extensions.Logging;
using Sportradar.Services.Entities;
using Sportradar.Services.Models;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;
using System.Text.Json;

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

        public async Task StartGameAsync(string homeTeamCode, string awayTeamCode)
        {
            try
            {
                _validator.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode);

                var homeTeam = await _gameRepository.GetTeamAsync(homeTeamCode);
                var awayTeam = await _gameRepository.GetTeamAsync(awayTeamCode);

                _validator.ValidateNotNullTeamEntity(homeTeam, awayTeam);

                var game = new Game
                {
                    HomeTeamCode = homeTeam!.TeamCode,
                    HomeTeamName = homeTeam.TeamName,
                    AwayTeamCode = awayTeam!.TeamCode,
                    AwayTeamName = awayTeam.TeamName,
                };

                await _gameRepository.AddAsync(game);
                await _gameRepository.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"ScoreBoardService > StartGame: homeTeamCode = {homeTeamCode}, awayTeamCode = {awayTeamCode}");
                throw;
            }
        }

        public async Task UpdateScoreAsync(UpdateScoreDto updateScoreModel)
        {
            try
            {
                _validator.ValidateUpdateScoreModel(updateScoreModel);

                await _gameRepository.UpdateScoreAsync(updateScoreModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"ScoreBoardService > UpdateScore: updateScoreModel = {JsonSerializer.Serialize(updateScoreModel)}");
                throw;
            }
        }

        public async Task FinishGameAsync(string homeTeamCode, string awayTeamCode)
        {
            try
            {
                _validator.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode);

                await _gameRepository.DeleteAsync(homeTeamCode, awayTeamCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"ScoreBoardService > FinishGame: homeTeamCode = {homeTeamCode}, awayTeamCode = {awayTeamCode}");
                throw;
            }
        }
    }
}
