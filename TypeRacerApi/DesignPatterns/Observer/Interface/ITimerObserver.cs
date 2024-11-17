using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Observer.Interface
{
    public interface IObserver
    {
        public bool isExpired { get; set; }
        ValueTask Update(IServiceProvider serviceProvider);
    }
}
