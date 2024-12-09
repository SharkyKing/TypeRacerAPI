using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public class GameTypeCollection : IIterableCollection<GameTypeClass>
    {
        private readonly List<GameTypeClass> _gameTypes;

        public GameTypeCollection(List<GameTypeClass> _gameTypes)
        {
            _gameTypes = _gameTypes;
        }

        public IIterator<GameTypeClass> CreateIterator()
        {
            return new GameTypeIterator(_gameTypes);
        }
    }
}
