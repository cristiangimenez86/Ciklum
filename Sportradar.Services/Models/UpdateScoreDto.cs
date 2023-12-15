namespace Sportradar.Services.Models
{
    public class UpdateScoreDto
    {
        public required string HomeTeamCode { get; set; }
        public required int HomeTeamScore { get; set; }
        public required string AwayTeamCode { get; set; }
        public required int AwayTeamScore { get; set; }
    }
}
