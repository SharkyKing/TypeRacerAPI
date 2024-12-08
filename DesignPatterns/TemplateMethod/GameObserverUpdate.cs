using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.State;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.TemplateMethod
{
    public class GameObserverUpdate : ObserverUpdateTemplate
    {
		protected override async Task<object?> GetEntityAsync(AppDbContext appDbContext, int id)
		{
			return await appDbContext.Games
				.AsNoTracking()
				.Include(g => g.Players)
				.SingleOrDefaultAsync(g => g.Id == id);
		}

		protected override bool IsEntityExpired(object entity)
		{
			var game = entity as GameClass;

			if (game == null || game.IsOver) {
				game?.SetState(new OverState());
			}
			else
			{
				game.SetState(new ActiveState());
			}
			game.HandleState();
			return game?.IsOver ?? false;
		}

		protected override void HandleEntityExpired(object entity)
		{
			var game = entity as GameClass;			
			if (game != null)
			{
				Console.WriteLine($"Game with ID {game.Id} is over.");
			}
		}

		protected override async Task UpdateEntityAsync(IHubContext<GameHub> hubContext, object entity)
		{
			var game = entity as GameClass;
			if (game != null)
			{
				await hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
			}
		}
	}
}
