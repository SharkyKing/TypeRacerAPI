using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public class WordStyleClassCollection : IIterableCollection<WordsStyleClass>
    {
        private readonly List<WordsStyleClass> _wordStyles;

        public WordStyleClassCollection(List<WordsStyleClass> wordStyles)
        {
            _wordStyles = wordStyles;
        }

        public IIterator<WordsStyleClass> CreateIterator()
        {
            return new WordsStyleClassIterator(_wordStyles);
        }
    }
}
