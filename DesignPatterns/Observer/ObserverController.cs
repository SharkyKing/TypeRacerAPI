using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Observer
{
    public class ObserverController
    {
        private static ObserverController _instance;
        private static readonly object _lock = new object();

        private IServiceProvider _serviceProvider;
        private List<IObserver> observers = new List<IObserver>();

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

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }

}
