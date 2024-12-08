using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.Hubs;
using System.Collections.Generic;
using TypeRacerAPI.DesignPatterns.Iterator;

namespace TypeRacerAPI.DesignPatterns.Observer
{
	public class ObserverController
	{
		private static ObserverController _instance;
		private static readonly object _lock = new object();
		private IServiceProvider _serviceProvider;
		private IterableCollection<IObserver> observers = new IterableCollection<IObserver>();

		private ObserverController() { }

		public static ObserverController GetInstance(IServiceProvider serviceProvider)
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new ObserverController();
						_instance._serviceProvider = serviceProvider;
					}
				}
			}
			else
			{
				_instance._serviceProvider = serviceProvider;
			}

			return _instance;
		}

		public void Attach(IObserver observer) => observers.Add(observer);

		public async ValueTask Notify(IServiceProvider serviceProvider)
		{
			IterableCollection<IObserver> observersTemp = new IterableCollection<IObserver>();
			IIterator<IObserver> iterator = observers.CreateIterator();

			while (iterator.HasNext())
			{
				observersTemp.Add(iterator.Next());
			}

			iterator = observers.CreateIterator();

			while (iterator.HasNext())
			{
				var observer = iterator.Next();
				_ = observer.Update(serviceProvider);

				if (observer.isExpired)
				{
					observersTemp.Remove(observer);
				}
			}
			observers.Clear();
			iterator = observersTemp.CreateIterator();

			while (iterator.HasNext())
			{
				observers.Add(iterator.Next());
			}
		}
	}
}