namespace Sportradar.Services.Entities
{
    public record Game
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public required string HomeTeamCode { get; set; }
        public required string HomeTeamName { get; set; }
        public int HomeTeamScore { get; set; } = 0;
        public required string AwayTeamCode { get; set; }
        public required string AwayTeamName { get; set; }
        public int AwayTeamScore { get; set; } = 0;
        public int TotalScore { get; set; } = 0;
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
