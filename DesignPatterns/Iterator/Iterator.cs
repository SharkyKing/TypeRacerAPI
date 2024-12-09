﻿namespace TypeRacerAPI.DesignPatterns.Iterator
{
	public interface IIterator<T>
	{
        bool HasNext();
        T Next();
        void Reset();
    }

}
