using TypeRacerAPI.DesignPatterns.Observer;

namespace TypeRacerAPI.DesignPatterns.Observer.Interface
{
    public interface IObserverController
    {

        void Attach(IObserver observer);

        ValueTask Notify(IServiceProvider serviceProvider);

}
}
