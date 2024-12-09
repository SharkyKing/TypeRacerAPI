using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public class PlayerIterator : IIterator<PlayerClass>
    {
        private readonly List<PlayerClass> _players;
        private int _currentIndex = -1;

        public PlayerIterator(List<PlayerClass> players)
        {
            _players = players;
        }

        public bool HasNext()
        {
            return _currentIndex + 1 < _players.Count;
        }

        public PlayerClass Next()
        {
            if (HasNext())
            {
                _currentIndex++;
                return _players[_currentIndex];
            }
            throw new InvalidOperationException("No more elements in the iterator.");
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }
}
