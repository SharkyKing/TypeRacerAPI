using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Services;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.DesignPatterns.Observer;
using System;
using static TypeRacerAPI.EnumClass;
using TypeRacerAPI.DesignPatterns.State;
using TypeRacerAPI.DesignPatterns.Iterator;

public class GameTimerService
{
    #region PROPERTIES
    private readonly ObserverController _observerController;
    private readonly GameService _gameService;

    #endregion

    public GameTimerService(AppDbContext context, IHubContext<GameHub> hubContext, GameService gameService, ObserverController observerController)
    {
        _observerController = observerController;
        _gameService = gameService;
    }
    #region GAME TIME CONTROL
    public async Task StartInitiatingGame(GameClass game, IServiceProvider _serviceProvider)
    {
        int countDown = ConstantService.StartGameCountdownSeconds;

        if (game != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();


                _appDbContext.Attach(game);
                game.IsOpen = false;
                await _appDbContext.SaveChangesAsync();

				while (countDown >= 0)
                {
                    await _hubContext.Clients.Group(game.Id.ToString())
                        .SendAsync(ConstantService.HubCalls[HubCall.TimerClient], new { countDown, msg = "Starting game" });
                    await Task.Delay(1000);
                    countDown--;
				}

				await _observerController.Notify(_serviceProvider);
				game.SetState(new TimerStartedState());
				game.HandleState();
				_ = StartGameTimer(game.Id, _serviceProvider);
				game.SetState(new TimerEndedState());
				game.HandleState();
			}
        }
    }
    public async Task StartGameTimer(int gameId, IServiceProvider _serviceProvider)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

            GameClass game = await _appDbContext.Games.SingleOrDefaultAsync(game => game.Id == gameId);

            game.StartTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            await _appDbContext.SaveChangesAsync();

            int time = ConstantService.GameCountdownSeconds;

            while (time >= 0)
            {
                string timeString = LogicHelper.CalculateTime(time);

                await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.TimerClient], new { countDown = timeString, msg = "Time left" });
                await Task.Delay(1000);

                game = await _appDbContext.Games
                    .AsNoTracking()
                    .SingleOrDefaultAsync(game => game.Id == gameId);

                if (game.IsOver)
                {
                    return;
                }

                time--;
            }

            PlayerClass player = new PlayerClass();
            await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = player });
        }
    }
	#endregion

	#region PLAYER TIME CONTROL
	public async Task PowerCoolDownTimer(int? playerId, string connectionId, IServiceProvider _serviceProvider)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

			var playerPowers = _appDbContext.PlayerPowerUses
				.Where(ppu => ppu.PlayerId == playerId && !ppu.IsUsed && !ppu.IsOnCooldown)
				.ToList();

			var cooldownCollection = new IterableCollection<KeyValuePair<PlayerPowerUseClass, int>>();

			foreach (var usePower in playerPowers)
			{
				var powerBase = _gameService.Powers.FirstOrDefault(ppb => ppb.Id == usePower.PlayerPowerId);
				if (powerBase != null)
				{
					cooldownCollection.Add(new KeyValuePair<PlayerPowerUseClass, int>(usePower, powerBase.CooldownTime));
					usePower.IsOnCooldown = true;
				}
			}

			await _appDbContext.SaveChangesAsync();

			while (true)
			{
				bool stillLeftTimers = false;

				var player = await _appDbContext.Players.AsNoTracking().SingleOrDefaultAsync(p => p.Id == playerId);
				if (player == null || !player.IsConnected) return;

				var iterator = cooldownCollection.CreateIterator();
				while (iterator.HasNext())
				{
					var pair = iterator.Next();
					if (pair.Value >= 0)
					{
						stillLeftTimers = true;

						await _hubContext.Clients.Client(connectionId).SendAsync(
							ConstantService.HubCalls[HubCall.CooldownTimer],
							new { powerUseId = pair.Key.Id, time = pair.Value }
						);

						// Update the collection by removing and re-adding the item with updated time.
						cooldownCollection.Remove(pair);
						cooldownCollection.Add(new KeyValuePair<PlayerPowerUseClass, int>(pair.Key, pair.Value - 1));
					}
					else
					{
						pair.Key.IsOnCooldown = false;
						await _appDbContext.SaveChangesAsync();

						// Remove the cooldown entry for completed timers.
						cooldownCollection.Remove(pair);
					}
				}

				if (!stillLeftTimers) break;

				await Task.Delay(1000);
			}
		}
	}

	#endregion
}
