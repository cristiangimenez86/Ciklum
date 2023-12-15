using Sportradar.Services.Entities;

namespace Sportradar.Services.Validators
{
    public interface IValidator
    {
        void ValidateNotNullOrEmptyCodes(string homeTeamCode, string awayTeamCode);
        void ValidateNotNullTeamEntity(Team? homeTeam, Team? awayTeam);
    }
}
