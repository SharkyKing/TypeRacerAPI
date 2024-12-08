using Microsoft.AspNetCore.SignalR;
using static TypeRacerAPI.EnumClass;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace TypeRacerAPI.DesignPatterns.Chain
{
    public class CheckAllPlayersFinishedHandler : IEndGameHandler
    {
        private IEndGameHandler _nextHandler;

        public void SetNextHandler(IEndGameHandler handler)
        {
            _nextHandler = handler;
        }

        public async Task HandleEndGameAsync(GameClass game, PlayerClass player, IServiceProvider serviceProvider, IHubContext<GameHub> hubContext)
        {
            var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

            List<PlayerClass> players = await _appDbContext.Players.Where(p => p.GameId == game.Id).ToListAsync();

            bool allFinished = players.All(p => p.Finished);

            if (allFinished)
            {
                if (_nextHandler != null)
                    await _nextHandler.HandleEndGameAsync(game, player, serviceProvider, hubContext);
            }
            else
            {
                Console.WriteLine("Not all players have finished.");
            }
        }
    }

    public class DetermineWinnerHandler : IEndGameHandler
    {
        private IEndGameHandler _nextHandler;

        public void SetNextHandler(IEndGameHandler handler)
        {
            _nextHandler = handler;
        }

        public async Task HandleEndGameAsync(GameClass game, PlayerClass player, IServiceProvider serviceProvider, IHubContext<GameHub> hubContext)
        {
            var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

            List<PlayerClass> players = await _appDbContext.Players.Where(p => p.GameId == game.Id).ToListAsync();
            PlayerClass winner = players.OrderBy(p => p.MistakeCount).FirstOrDefault(p => p.Finished);

            if (winner != null)
            {
                if (_nextHandler != null)
                    await _nextHandler.HandleEndGameAsync(game, winner, serviceProvider, hubContext);
            }
            else
            {
                Console.WriteLine("No winner determined.");
            }
        }
    }

    public class SendGameOverMessageHandler : IEndGameHandler
    {
        private IEndGameHandler _nextHandler;

        public void SetNextHandler(IEndGameHandler handler)
        {
            _nextHandler = handler;
        }

        public async Task HandleEndGameAsync(GameClass game, PlayerClass player, IServiceProvider serviceProvider, IHubContext<GameHub> hubContext)
        {
            await hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = player });
        }
    }

}
