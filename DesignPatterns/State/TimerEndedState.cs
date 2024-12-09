using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.State
{
    public class TimerEndedState : IGameState
    {
        public void Handle(GameClass game, IServiceScopeFactory serviceProvider)
        {
            // Logic for handling over state
            // Kreipimasis i db StartTime = 0
            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                game.StartTime = 0;
            }
        }
	}
}
