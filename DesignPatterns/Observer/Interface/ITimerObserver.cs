using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Observer.Interface
{
    public interface IObserver
    {
        ValueTask Update();
    }
}
