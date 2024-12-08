using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.State
{
    public class ActiveState : IGameState
    {
        public void Handle(GameClass game, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                game.IsOpen = true;
                game.IsOver = false;
            }
        }
    }
}
