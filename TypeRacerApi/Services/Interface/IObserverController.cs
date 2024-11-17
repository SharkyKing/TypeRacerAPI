using TypeRacerAPI.DesignPatterns.Observer;

namespace TypeRacerAPI.Services.Interface
{
	public interface IObserverController
	{
		IObserverController GetInstance();

		void Attach(DesignPatterns.Observer.Interface.IObserver observer);

		ValueTask Notify(IServiceProvider serviceProvider);

	}	
}
