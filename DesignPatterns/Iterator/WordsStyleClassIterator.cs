using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
	public class WordsStyleClassIterator : IIterator<WordsStyleClass>
	{
        private readonly List<WordsStyleClass> _wordStyles;
        private int _currentIndex = -1;

        public WordsStyleClassIterator(List<WordsStyleClass> wordStyles)
        {
            _wordStyles = wordStyles;
        }

        public bool HasNext()
        {
            return _currentIndex + 1 < _wordStyles.Count;
        }


        public WordsStyleClass Next()
        {
            if (HasNext())
            {
                _currentIndex++;
                return _wordStyles[_currentIndex];
            }
            throw new InvalidOperationException("No more elements in the iterator.");
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }

}
