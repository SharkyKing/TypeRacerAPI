namespace TypeRacerAPI.DesignPatterns.Iterator
{
    public interface ICustomIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
