using Sportradar.Services.Entities;
using Sportradar.Services.Models;

namespace Sportradar.Services.Validators
{
    public interface IValidator
    {
        void ValidateNotNullOrEmptyCode(string code, string parameterName);
        void ValidateNotNullTeamEntity(Team? team);
        void ValidateUpdateScoreModel(UpdateScoreDto updateScoreModel);
    }
}
