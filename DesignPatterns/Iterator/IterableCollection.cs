namespace TypeRacerAPI.DesignPatterns.Iterator
{
	public class IterableCollection<T>
	{
		private readonly List<T> _items = new List<T>();

		public void Add(T item) => _items.Add(item);

		public void Remove(T item) => _items.Remove(item);

		public void Clear() => _items.Clear();

		public IIterator<T> CreateIterator() => new ListIterator(_items);

		private class ListIterator : IIterator<T>
		{
			private readonly List<T> _collection;
			private int _index = 0;

			public ListIterator(List<T> collection) => _collection = collection;

			public bool HasNext() => _index < _collection.Count;

			public T Next() => _collection[_index++];
		}
	}

}
