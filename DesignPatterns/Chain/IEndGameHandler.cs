using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Chain
{
    public interface IEndGameHandler
    {
        Task HandleEndGameAsync(GameClass game, PlayerClass player, IServiceProvider serviceProvider, IHubContext<GameHub> hubContext);
        void SetNextHandler(IEndGameHandler handler);
    }
}
