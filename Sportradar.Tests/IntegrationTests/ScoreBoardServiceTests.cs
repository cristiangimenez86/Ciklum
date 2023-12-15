using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sportradar.Services;
using Sportradar.Services.Data;
using Sportradar.Services.DbContexts;
using Sportradar.Services.Entities;
using Sportradar.Services.Models;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;

namespace Sportradar.Tests.IntegrationTests
{
    public class ScoreBoardServiceTests
    {
        private readonly IScoreBoardService _scoreBoardService;
        private readonly IGameRepository _gameRepository;

        public ScoreBoardServiceTests()
        {
            var options = new DbContextOptionsBuilder<ScoreBoardDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var scoreBoardDbContext = new ScoreBoardDbContext(options);
            _gameRepository = new GameRepository(scoreBoardDbContext);
            var validator = new Validator();
            var loggerMock = new Mock<ILogger<ScoreBoardService>>();
            
            _scoreBoardService = new ScoreBoardService(_gameRepository, validator, loggerMock.Object);
        }

        [Theory]
        [InlineData("ARG", "MEX")]
        [InlineData("BOL", "PAR")]
        [InlineData("CHI", "COL")]
        public async Task StartGameAsync_CreatesANewGame_WhenValidPairOfTeamCodes(string homeTeamCode, string awayTeamCode)
        {
            //Arrange
            var homeTeamName = TeamData.GetTeamCollection().Single(x => x.TeamCode == homeTeamCode).TeamName;
            var awayTeamName = TeamData.GetTeamCollection().Single(x => x.TeamCode == awayTeamCode).TeamName;

            //Act
            await _scoreBoardService.StartGameAsync(homeTeamCode, awayTeamCode);

            //Assert
            var result = await _gameRepository.GetAsync();
            var game = result.Single();

            Assert.Equal(homeTeamCode, game.HomeTeamCode);
            Assert.Equal(awayTeamCode, game.AwayTeamCode);
            Assert.Equal(homeTeamName, game.HomeTeamName);
            Assert.Equal(awayTeamName, game.AwayTeamName);
            Assert.Equal(0, game.HomeTeamScore);
            Assert.Equal(0, game.AwayTeamScore);
            Assert.Equal(0, game.TotalScore);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 0)]
        public async Task UpdateScoreAsync_UpdatesTheScoreOfAMatch_WhenValidData(int homeTeamScore, int awayTeamScore)
        {
            //Arrange
            await AddGamesToDatabase();

            //Act
            var updateScoreModel = new UpdateScoreDto { HomeTeamCode = "ARG", HomeTeamScore = homeTeamScore, AwayTeamCode = "BRA", AwayTeamScore = awayTeamScore };
            await _scoreBoardService.UpdateScoreAsync(updateScoreModel);

            //Assert
            var result = await _gameRepository.GetAsync();
            var game = result.Single(x=>x.HomeTeamCode.Equals("ARG") && x.AwayTeamCode.Equals("BRA"));

            Assert.Equal(homeTeamScore, game.HomeTeamScore);
            Assert.Equal(awayTeamScore, game.AwayTeamScore);
        }

        [Fact]
        public async Task FinishGameAsync_RemovesAMatchFromTheScoreBoard_WhenValidPairOfTeamCodes()
        {
            //Arrange
            await AddGamesToDatabase();
            var gamesBeforeDelete = await _gameRepository.GetAsync();

            //Act
            await _scoreBoardService.FinishGameAsync("ARG", "BRA");

            //Assert
            var actualGames = await _gameRepository.GetAsync();
            Assert.Equal(2, gamesBeforeDelete.Count());
            Assert.Single(actualGames);
        }

        [Fact]
        public async Task GetSummaryAsync_ReturnsAListOfGameSummaryDto()
        {
            //Arrange
            await AddGamesToDatabase();

            //Act
            var result = await _scoreBoardService.GetSummaryAsync();
            var actualSummary = result.ToList();

            //Assert
            Assert.NotEmpty(actualSummary);
        }

        private async Task AddGamesToDatabase()
        {
            var entity1 = new Game { HomeTeamCode = "ARG", HomeTeamName = "Argentina", HomeTeamScore = 3, AwayTeamCode = "BRA", AwayTeamName = "Brazil", AwayTeamScore = 0 };
            var entity2 = new Game { HomeTeamCode = "CHI", HomeTeamName = "Chile", HomeTeamScore = 4, AwayTeamCode = "COL", AwayTeamName = "Colombia", AwayTeamScore = 2 };
            await _gameRepository.AddAsync(entity1);
            await _gameRepository.AddAsync(entity2);
            await _gameRepository.CommitAsync();
        }
    }
}
