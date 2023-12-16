using Microsoft.Extensions.Logging;
using Sportradar.Services.Entities;
using Sportradar.Services.Models;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;
using System.Text.Json;
using Sportradar.Services.Exceptions;

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
                _validator.ValidateNotNullOrEmptyCode(homeTeamCode, "HomeTeamCode");
                _validator.ValidateNotNullOrEmptyCode(awayTeamCode, "AwayTeamCode");

                var homeTeam = await _gameRepository.GetTeamAsync(homeTeamCode);
                var awayTeam = await _gameRepository.GetTeamAsync(awayTeamCode);

                _validator.ValidateNotNullTeamEntity(homeTeam);
                _validator.ValidateNotNullTeamEntity(awayTeam);

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
                _logger.LogError(e, $"ScoreBoardService > StartGameAsync: homeTeamCode = {homeTeamCode}, awayTeamCode = {awayTeamCode}");
                throw new ScoreBoardException(e.Message, e);
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
                _logger.LogError(e, $"ScoreBoardService > UpdateScoreAsync: updateScoreModel = {JsonSerializer.Serialize(updateScoreModel)}");
                throw new ScoreBoardException(e.Message, e);
            }
        }

        public async Task FinishGameAsync(string homeTeamCode, string awayTeamCode)
        {
            try
            {
                _validator.ValidateNotNullOrEmptyCode(homeTeamCode, "HomeTeamCode");
                _validator.ValidateNotNullOrEmptyCode(awayTeamCode, "AwayTeamCode");

                await _gameRepository.DeleteAsync(homeTeamCode, awayTeamCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"ScoreBoardService > FinishGameAsync: homeTeamCode = {homeTeamCode}, awayTeamCode = {awayTeamCode}");
                throw new ScoreBoardException(e.Message, e);
            }
        }

        public async Task<IEnumerable<GameSummaryDto>> GetSummaryAsync()
        {
            try
            {
                var games = await _gameRepository.GetAsync();

                return games.Select(game => new GameSummaryDto
                {
                    HomeTeam = game.HomeTeamName,
                    HomeTeamScore = game.HomeTeamScore,
                    AwayTeam = game.AwayTeamName,
                    AwayTeamScore = game.AwayTeamScore
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"ScoreBoardService > GetSummaryAsync");
                throw new ScoreBoardException(e.Message, e);
            }
        }
    }
}
