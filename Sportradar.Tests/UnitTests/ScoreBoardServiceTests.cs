using Microsoft.Extensions.Logging;
using Moq;
using Sportradar.Services;
using Sportradar.Services.Data;
using Sportradar.Services.Entities;
using Sportradar.Services.Models;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;

namespace Sportradar.Tests.UnitTests
{
    public class ScoreBoardServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<IValidator> _validatorMock;

        private readonly ScoreBoardService _scoreBoardService;
        public ScoreBoardServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _validatorMock = new Mock<IValidator>();
            var loggerMock = new Mock<ILogger<ScoreBoardService>>();

            _scoreBoardService = new ScoreBoardService(_gameRepositoryMock.Object, _validatorMock.Object, loggerMock.Object);
        }

        [Theory]
        [InlineData("ARG", "MEX")]
        [InlineData("BOL", "PAR")]
        [InlineData("CHI", "COL")]
        public async Task StartGameAsync_CreatesANewGame_WhenValidPairOfTeamCodes(string homeTeamCode, string awayTeamCode)
        {
            //Arrange
            var homeTeam = TeamData.GetTeamCollection().FirstOrDefault(x => x.TeamCode == homeTeamCode);
            var awayTeam = TeamData.GetTeamCollection().FirstOrDefault(x => x.TeamCode == awayTeamCode);

            _gameRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Game>()));
            _gameRepositoryMock.Setup(repo => repo.CommitAsync());
            _gameRepositoryMock.Setup(repo => repo.GetTeamAsync(homeTeamCode)).ReturnsAsync(homeTeam);
            _gameRepositoryMock.Setup(repo => repo.GetTeamAsync(awayTeamCode)).ReturnsAsync(awayTeam);

            _validatorMock.Setup(v => v.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode));
            _validatorMock.Setup(v => v.ValidateNotNullTeamEntity(homeTeam, awayTeam));

            //Act
            await _scoreBoardService.StartGameAsync(homeTeamCode, awayTeamCode);

            //Assert
            _gameRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Game>()), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetTeamAsync(homeTeamCode), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetTeamAsync(awayTeamCode), Times.Once);
            _validatorMock.Verify(v => v.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode), Times.Once);
            _validatorMock.Verify(v => v.ValidateNotNullTeamEntity(homeTeam, awayTeam), Times.Once);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 0)]
        public async Task UpdateScoreAsync_UpdatesTheScoreOfAMatch_WhenValidData(int homeTeamScore, int awayTeamScore)
        {
            //Arrange
            var updateScoreModel = new UpdateScoreDto
            {
                HomeTeamCode = "ARG",
                HomeTeamScore = homeTeamScore,
                AwayTeamCode = "BRA",
                AwayTeamScore = awayTeamScore
            };
            _validatorMock.Setup(v => v.ValidateUpdateScoreModel(updateScoreModel));
            _gameRepositoryMock.Setup(repo => repo.UpdateScoreAsync(updateScoreModel));

            //Act
            await _scoreBoardService.UpdateScoreAsync(updateScoreModel);

            //Assert
            _validatorMock.Verify(v => v.ValidateUpdateScoreModel(updateScoreModel), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.UpdateScoreAsync(updateScoreModel), Times.Once);
        }

        [Theory]
        [InlineData("ARG", "MEX")]
        public async Task FinishGameAsync_RemovesAMatchFromTheScoreBoard_WhenValidPairOfTeamCodes(string homeTeamCode, string awayTeamCode)
        {
            //Arrange
            _validatorMock.Setup(v => v.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode));
            _gameRepositoryMock.Setup(repo => repo.DeleteAsync(homeTeamCode, awayTeamCode));

            //Act
            await _scoreBoardService.FinishGameAsync(homeTeamCode, awayTeamCode);

            //Assert
            _validatorMock.Verify(v => v.ValidateNotNullOrEmptyCodes(homeTeamCode, awayTeamCode), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.DeleteAsync(homeTeamCode, awayTeamCode), Times.Once);
        }

        [Fact]
        public async Task GetSummaryAsync_ReturnsAListOfGameSummaryDto()
        {
            //Arrange
            var games = new List<Game>()
            {
                new() { HomeTeamCode = "ARG", HomeTeamName = "Argentina", HomeTeamScore = 3, AwayTeamCode = "BRA", AwayTeamName = "Brazil", AwayTeamScore = 0 },
                new() { HomeTeamCode = "URU", HomeTeamName = "Uruguay", HomeTeamScore = 2, AwayTeamCode = "MEX", AwayTeamName = "Mexico", AwayTeamScore = 1 }
            };
            _gameRepositoryMock.Setup(repo => repo.GetAsync()).ReturnsAsync(games);

            var expectedSummary = new List<GameSummaryDto>
            {
                new() { HomeTeam = "Argentina", HomeTeamScore = 3, AwayTeam = "Brazil", AwayTeamScore = 0 },
                new() { HomeTeam = "Uruguay", HomeTeamScore = 2, AwayTeam = "Mexico", AwayTeamScore = 1 },
            };

            //Act
            var result = await _scoreBoardService.GetSummaryAsync();
            var actualSummary = result.ToList();

            //Assert
            Assert.NotEmpty(actualSummary);
            Assert.Equal(2, expectedSummary.Count);

            var expectedFirstElement = expectedSummary.First();
            var actualFirstElement = actualSummary.First();
            Assert.Equal(expectedFirstElement.HomeTeam, actualFirstElement.HomeTeam);
            Assert.Equal(expectedFirstElement.HomeTeamScore, actualFirstElement.HomeTeamScore);
            Assert.Equal(expectedFirstElement.AwayTeam, actualFirstElement.AwayTeam);
            Assert.Equal(expectedFirstElement.AwayTeamScore, actualFirstElement.AwayTeamScore);
        }
    }
}
