using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Validators
{
    public class Validator : IValidator
    {
        private const int MaxScoreCount = 100;
        public void ValidateNotNullOrEmptyCode(string code, string parameterName)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.");
            }
        }

        public void ValidateNotNullTeamEntity(Team? team)
        {
            if (team == null)
            {
                throw new InvalidOperationException($"Team not found: {team}.");
            }
        }

        public void ValidateUpdateScoreModel(UpdateScoreDto updateScoreModel)
        {
            ValidateNotNullOrEmptyCode(updateScoreModel.HomeTeamCode, "HomeTeamCode");
            ValidateNotNullOrEmptyCode(updateScoreModel.AwayTeamCode, "AwayTeamCode");

            ValidateNotNegative(updateScoreModel.HomeTeamScore, "HomeTeamScore");
            ValidateNotNegative(updateScoreModel.AwayTeamScore, "AwayTeamScore");

            ValidateNotGreaterThan(updateScoreModel.HomeTeamScore, "HomeTeamScore");
            ValidateNotGreaterThan(updateScoreModel.AwayTeamScore, "AwayTeamScore");
        }

        private static void ValidateNotNegative(int number, string parameterName)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException($"{parameterName} cannot be negative");
            }
        }

        private static void ValidateNotGreaterThan(int number, string parameterName)
        {
            if (number > MaxScoreCount)
            {
                throw new InvalidOperationException($"{parameterName} cannot be greater than {MaxScoreCount}");
            }
        }
    }
}
