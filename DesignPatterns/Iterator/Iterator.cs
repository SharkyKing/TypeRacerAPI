namespace TypeRacerAPI.DesignPatterns.Iterator
{
	public interface IIterator<T>
	{
		bool HasNext(); // Checks if there are more elements to iterate over.
		T Next();       // Retrieves the next element in the collection.
	}

}
