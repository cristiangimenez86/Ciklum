using Sportradar.Services.Entities;

namespace Sportradar.Services.Validators
{
    public class Validator : IValidator
    {
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
    }
}
