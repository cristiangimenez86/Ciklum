namespace Sportradar.Services.Entities
{
    public class Team
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string TeamCode { get; set; }
        public required string TeamName { get; set; }
    }
}
