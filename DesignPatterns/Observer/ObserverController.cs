using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.Hubs;
using System.Collections.Generic;
using TypeRacerAPI.DesignPatterns.Iterator;
using Microsoft.AspNetCore.Http.Features;

namespace TypeRacerAPI.DesignPatterns.Observer
{
	public class ObserverController
	{
		private static ObserverController _instance;
		private static readonly object _lock = new object();
		private IServiceScopeFactory _serviceProvider;
		private CustomIterableCollection<IObserver> observers = new CustomIterableCollection<IObserver>();

		private ObserverController() { }

		public static ObserverController GetInstance(IServiceScopeFactory serviceProvider)
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

		public async ValueTask Notify(IServiceScopeFactory serviceProvider)
		{
			CustomIterableCollection<IObserver> observersTemp = new CustomIterableCollection<IObserver>();
            ICustomIterator<IObserver> iterator = observers.CreateIterator();

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