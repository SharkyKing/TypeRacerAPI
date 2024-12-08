namespace TypeRacerAPI.DesignPatterns.Memento
{
    public class MessageMementoController
    {
        private Dictionary<int, MessageMemento> _memento;

        public void SaveValue(int id, string message)
        {
            if (_memento == null)
            {
                _memento = new Dictionary<int, MessageMemento>()
                {
                    {id, new MessageMemento(message) }
                };
            }
            else
            {
                if (_memento.ContainsKey(id))
                {
                    _memento[id] = new MessageMemento(message);
                }
                else
                {
                    _memento.Add(id, new MessageMemento(message));
                }
            }


        }

        public MessageMemento GetLastValue(int id)
        {

            if (_memento.ContainsKey(id))
            {
                return _memento[id];
            }
            else
            {
                return null;
            }
        }
    }
}
