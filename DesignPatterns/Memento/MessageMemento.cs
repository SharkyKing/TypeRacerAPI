namespace TypeRacerAPI.DesignPatterns.Memento
{
    public class MessageMemento
    {
        public string Message { get; private set; }

        public MessageMemento(string message)
        {
            Message = message;
        }
    }
}
