using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Validators
{
    public interface IValidator
    {
        void ValidateNotNullOrEmptyCodes(string homeTeamCode, string awayTeamCode);
        void ValidateNotNullTeamEntity(Team? homeTeam, Team? awayTeam);
        void ValidateUpdateScoreModel(UpdateScoreDto updateScoreModel);
    }
}
