using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Mediator;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.TemplateMethod;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.Observer
{
    public class GameObserver : IObserver
    {
        public int gameId;

        public bool isExpired { get; set; }
		private readonly IMediator _mediator;
		public GameObserver(IMediator mediator)
		{
			_mediator = mediator;
		}

		public void SetGameId(int id)
        {
            gameId = id;
        }

        //public async ValueTask Update0(IServiceScopeFactory _serviceProvider)
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //            var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
        //            var game = await _appDbContext.Games
        //                .AsNoTracking()
        //                .Include(g => g.Players)
        //                .SingleOrDefaultAsync(g => g.Id == gameId);
        //
        //            if (game == null)
        //            {
        //                Console.WriteLine($"Game with ID {gameId} not found.");
        //                return;
        //            }
        //            else
        //            {
        //                if (game.IsOver)
        //                {
        //                    isExpired = true;
        //                }
        //            }
        //
        //            await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error updating game with ID {gameId}: {ex.Message}");
        //    }
        //}

        public async ValueTask Update(IServiceScopeFactory serviceProvider)
		{
			try
			{
				await _mediator.NotifyAsync(this, "Update");
				//var gameUpdate = new GameObserverUpdate();
				//await gameUpdate.UpdateAsync(serviceProvider, gameId);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating game with ID {gameId}: {ex.Message}");
			}
		}
	}

}
