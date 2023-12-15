using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Validators
{
    public class Validator : IValidator
    {
        private const int MaxScoreCount = 100;
        public void ValidateNotNullOrEmptyCodes(string homeTeamCode, string awayTeamCode)
        {
            if (string.IsNullOrEmpty(homeTeamCode))
            {
                throw new ArgumentException($"Argument {nameof(homeTeamCode)} cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(awayTeamCode))
            {
                throw new ArgumentException($"Argument {nameof(awayTeamCode)} cannot be null or empty.");
            }
        }

        public void ValidateNotNullTeamEntity(Team? homeTeam, Team? awayTeam)
        {
            if (homeTeam == null)
            {
                throw new InvalidOperationException($"Team not found: {homeTeam}.");
            }

            if (awayTeam == null)
            {
                throw new InvalidOperationException($"Team not found: {awayTeam}.");
            }
        }

        public void ValidateUpdateScoreModel(UpdateScoreDto updateScoreModel)
        {
            ValidateNotNullOrEmptyCodes(updateScoreModel.HomeTeamCode, updateScoreModel.AwayTeamCode);

            if (updateScoreModel.HomeTeamScore < 0)
            {
                throw new InvalidOperationException($"HomeTeamScore cannot be negative");
            }

            if (updateScoreModel.HomeTeamScore > MaxScoreCount)
            {
                throw new InvalidOperationException($"HomeTeamScore cannot be greater than {MaxScoreCount}");
            }

            if (updateScoreModel.AwayTeamScore < 0)
            {
                throw new InvalidOperationException($"AwayTeamScore cannot be negative");
            }

            if (updateScoreModel.AwayTeamScore > MaxScoreCount)
            {
                throw new InvalidOperationException($"AwayTeamScore cannot be greater than {MaxScoreCount}");
            }
        }
    }
}
