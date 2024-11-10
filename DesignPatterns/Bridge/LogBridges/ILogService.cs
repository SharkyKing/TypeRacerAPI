namespace TypeRacerAPI.DesignPatterns.Bridge.LogBridges
{
    public interface ILogService
    {
        ValueTask Log(string message, int? gameId, int? playerId, IServiceProvider serviceProvider);
    }
}
