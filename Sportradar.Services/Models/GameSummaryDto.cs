namespace Sportradar.Services.Models
{
    public class GameSummaryDto
    {
        public required string HomeTeam { get; set; }
        public required int HomeTeamScore { get; set; }
        public required string AwayTeam { get; set; }
        public required int AwayTeamScore { get; set; }
    }
}
