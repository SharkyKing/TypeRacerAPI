using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public class GameTypeIterator : IIterator<GameTypeClass>
    {
        private readonly List<GameTypeClass> _gameTypes;
        private int _currentIndex = -1;

        public GameTypeIterator(List<GameTypeClass> gameTypes)
        {
            _gameTypes = gameTypes;
        }

        public bool HasNext()
        {
            return _currentIndex + 1 < _gameTypes.Count;
        }

        public GameTypeClass Next()
        {
            if (HasNext())
            {
                _currentIndex++;
                return _gameTypes[_currentIndex];
            }
            throw new InvalidOperationException("No more elements in the iterator.");
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }
}
