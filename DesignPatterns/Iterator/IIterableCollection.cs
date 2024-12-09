namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public interface IIterableCollection<T>
    {
        IIterator<T> CreateIterator();
    }

}
