using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.Mediator
{
    public class GameMediator : IMediator
    {
        private readonly IServiceScopeFactory _serviceProvider;

        public GameMediator(IServiceScopeFactory serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NotifyAsync(object sender, string ev)
        {
            if (sender is GameObserver gameObserver && ev == "Update")
            {
                await UpdateGameAsync(gameObserver.gameId);
            }
            else if (sender is PlayerObserver playerObserver && ev == "Update")
            {
                await UpdatePlayerAsync(playerObserver.playerId);
            }
            else if (sender is GameHub gameHub && ev == "NotifyPlayers")
            {
                //await NotifyPlayersAsync();
            }
        }

        private async Task UpdateGameAsync(int gameId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                var game = await _appDbContext.Games
                    .AsNoTracking()
                    .Include(g => g.Players)
                    .SingleOrDefaultAsync(g => g.Id == gameId);

                if (game == null)
                {
                    Console.WriteLine($"Game with ID {gameId} not found.");
                    return;
                }

                if (game.IsOver)
                {
                    Console.WriteLine($"Game with ID {game.Id} is over.");
                }

                await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
            }
        }

        private async Task UpdatePlayerAsync(int playerId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                var player = await _appDbContext.Players
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == playerId);

                if (player == null)
                {
                    Console.WriteLine($"Player with ID {playerId} not found.");
                    return;
                }

                if (player.Game.IsOver)
                {
                }

                //await _hubContext.Clients.Group(player.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdatePlayer], player);
            }
        }

        private async Task NotifyPlayersAsync(int gameId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                var game = await _appDbContext.Games
                    .AsNoTracking()
                    .Include(g => g.Players)
                    .SingleOrDefaultAsync(g => g.Id == gameId);

                if (game == null)
                {
                    Console.WriteLine($"Game with ID {gameId} not found.");
                    return;
                }

                //foreach (var player in game.Players)
                //{
                //    await _hubContext.Clients.Group(player.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.NotifyPlayer], player);
                //}
            }
        }
    }
}
