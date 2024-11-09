using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Observer
{
    public class GameObserver : IObserver
    {
        private int gameId;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IServiceProvider _serviceProvider;

        public GameObserver(IHubContext<GameHub> hubContext, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        public void SetGameId(int id)
        {
            gameId = id;
        }

        public async ValueTask Update()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var game = await _appDbContext.Games
                        .SingleOrDefaultAsync(g => g.Id == gameId);

                    if (game == null)
                    {
                        Console.WriteLine($"Game with ID {gameId} not found.");
                        return;
                    }

                    await _hubContext.Clients.Group(game.Id.ToString()).SendAsync("UpdateGame", game);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating game with ID {gameId}: {ex.Message}");
            }
        }
    }

}
