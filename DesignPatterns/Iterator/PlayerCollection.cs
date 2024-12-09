using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public class PlayerCollection : IIterableCollection<PlayerClass>
    {
        private readonly List<PlayerClass> _players;

        public PlayerCollection(List<PlayerClass> players)
        {
            _players = players;
        }

        public IIterator<PlayerClass> CreateIterator()
        {
            return new PlayerIterator(_players);
        }
    }
}
