namespace Sportradar.Services.Entities
{
    public record Team
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string TeamCode { get; set; }
        public required string TeamName { get; set; }
    }
}
