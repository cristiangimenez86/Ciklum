using Microsoft.Extensions.Logging;
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

    }
}
