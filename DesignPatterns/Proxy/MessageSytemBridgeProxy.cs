using TypeRacerAPI.DesignPatterns.Bridge;

namespace TypeRacerAPI.DesignPatterns.Proxy
{
    public class MessageSystemBridgeProxy : IMessageSystemBridge
    {
        private readonly MessageSystemBridge _realBridge;

        public MessageSystemBridgeProxy(MessageSystemBridge realBridge)
        {
            _realBridge = realBridge;
        }

        public async ValueTask SendMessageToGame(string message)
        {
            Console.WriteLine("Logging message: " + message);
            await _realBridge.SendMessageToGame(message);
        }
    }

}
