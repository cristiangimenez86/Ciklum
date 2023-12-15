namespace Sportradar.Services
{
    public interface IScoreBoardService
    {
        Task StartGame(string homeTeamCode, string awayTeamCode);
    }
}
