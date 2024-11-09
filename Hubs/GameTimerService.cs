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

public class GameTimerService
{
    private readonly AppDbContext _appDbContext;
    private readonly IHubContext<GameHub> _hubContext;
    private readonly ObserverController _observerController;
    private readonly GameService _gameService;

    public GameTimerService(AppDbContext context, IHubContext<GameHub> hubContext, GameService gameService, ObserverController observerController)
    {
        _appDbContext = context;
        _hubContext = hubContext;
        _observerController = observerController;
        _gameService = gameService;  // Injected GameService instance
    }

    public async Task StartGameTimer(GameClass game)
    {
        if (game != null)
        {
            game.StartTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            await _appDbContext.SaveChangesAsync();

            int time = ConstantService.GameCountdownSeconds;

            while (time >= 0)
            {
                string timeString = LogicHelper.CalculateTime(time);

                await _hubContext.Clients.Group(game.Id.ToString()).SendAsync("timerClient", new { countDown = timeString, msg = "Time left" });
                await Task.Delay(1000); 
                time--;
            }

            PlayerClass player = new PlayerClass();
            await _hubContext.Clients.Group(game.Id.ToString()).SendAsync("done", new { game, playerWon = player });
        }
    }

    public async Task PowerCoolDownTimer(int playerId, string connectionId)
    {
        bool allTimersFinished = false;

        var playerPowers = _appDbContext.PlayerPowerUses.Where(ppu => ppu.PlayerId == playerId).ToList();

        Dictionary<PlayerPowerUseClass, int> playerPowerUseCoolDownLeft = new Dictionary<PlayerPowerUseClass, int>();

        foreach (PlayerPowerUseClass usePower in playerPowers)
        {
            var powerBase = _gameService.Powers.Where(ppb => ppb.Id == usePower.PlayerPowerId).FirstOrDefault();
            int time = powerBase.CooldownTime;

            if (!playerPowerUseCoolDownLeft.ContainsKey(usePower))
            {
                playerPowerUseCoolDownLeft.Add(usePower, time);
                usePower.IsOnCooldown = true;
            }
        }

        await _appDbContext.SaveChangesAsync();

        while (!allTimersFinished)
        {
            bool stillLeftTimers = false;
            foreach(KeyValuePair<PlayerPowerUseClass, int> pair in playerPowerUseCoolDownLeft)
            {
                if(pair.Value >= 0)
                {
                    stillLeftTimers = true;
                    int timeCurrent = playerPowerUseCoolDownLeft[pair.Key];

                    await _hubContext
                        .Clients
                        .Client(connectionId)
                        .SendAsync("cooldowntimer",
                            new { powerUseId = pair.Key.Id, time = timeCurrent }
                        );

                    playerPowerUseCoolDownLeft[pair.Key] = pair.Value - 1;
                }
                else
                {
                    pair.Key.IsOnCooldown = false;
                    await _appDbContext.SaveChangesAsync();
                }
            }

            await Task.Delay(1000);
            allTimersFinished = !stillLeftTimers;
        }
    }
}
