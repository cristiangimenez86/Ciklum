﻿using Microsoft.Extensions.Logging;
using Moq;
using Sportradar.Services;
using Sportradar.Services.Data;
using Sportradar.Services.Entities;
using Sportradar.Services.Repositories;
using Sportradar.Services.Validators;

namespace Sportradar.Tests.UnitTests
{
    public class ScoreBoardServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<IValidator> _validatorMock;
        private readonly Mock<ILogger<ScoreBoardService>> _loggerMock;

        private readonly ScoreBoardService _scoreBoardService;
        public ScoreBoardServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _validatorMock = new Mock<IValidator>();
            _loggerMock = new Mock<ILogger<ScoreBoardService>>();

            _scoreBoardService = new ScoreBoardService(_gameRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object);

            //SetupLoggerMock(_loggerMock);
        }

        [Theory]
        [InlineData("ARG", "MEX")]
        public async Task StartGame_CreatesANewGame_WhenValidPairOfTeamCodes(string homeTeamCode, string awayTeamCode)
        {
            //Arrange
            var homeTeam = TeamData.GetTeamCollection().FirstOrDefault(x => x.TeamCode == homeTeamCode);
            var awayTeam = TeamData.GetTeamCollection().FirstOrDefault(x => x.TeamCode == awayTeamCode);

            _gameRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Game>()));
            _gameRepositoryMock.Setup(repo => repo.CommitAsync());
            _gameRepositoryMock.Setup(repo => repo.GetTeam(homeTeamCode)).ReturnsAsync(homeTeam);
            _gameRepositoryMock.Setup(repo => repo.GetTeam(awayTeamCode)).ReturnsAsync(awayTeam);

            //Act
            await _scoreBoardService.StartGame(homeTeamCode, awayTeamCode);

            //Assert
            _gameRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Game>()), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.CommitAsync(), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetTeam(homeTeamCode), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetTeam(awayTeamCode), Times.Once);
        }
    }
}
