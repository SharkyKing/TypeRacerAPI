using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.TemplateMethod
{
    public sealed class PlayerObserverUpdate : ObserverUpdateTemplate
    {
        protected override async Task<object?> GetEntityAsync(AppDbContext appDbContext, int id)
        {
            return await appDbContext.Players
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        protected override bool IsEntityExpired(object entity, IServiceProvider serviceProvider)
        {
            var player = entity as PlayerClass;
            return player?.Game.IsOver ?? false;
        }

        protected override void HandleEntityExpired(object entity)
        {
            var player = entity as PlayerClass;
            if (player != null)
            {
                //Console.WriteLine($"Player with ID {player.Id} is inactive.");
            }
        }

        protected override async Task UpdateEntityAsync(IHubContext<Hubs.GameHub> hubContext, object entity)
        {
            var player = entity as PlayerClass;
            if (player != null)
            {
                try
                {
					await hubContext.Clients.Group(player.Game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.TimerClient], player);
				}
                catch
                { }
            }
        }
    }
}
