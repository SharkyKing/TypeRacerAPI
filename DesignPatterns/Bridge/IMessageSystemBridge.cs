namespace TypeRacerAPI.DesignPatterns.Bridge
{
    public interface IMessageSystemBridge
    {
        ValueTask SendMessageToGame(string message);
    }
}
