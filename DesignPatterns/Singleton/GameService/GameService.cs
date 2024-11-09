using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Controllers.ControllerHelperClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.DesignPatterns.Singleton.GameService
{
    public class GameService
    {
        private AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private static GameService? _instance;

        private static readonly object _lock = new object();
        public List<GameTypeClass> GameTypes { get; private set; }
        public List<GameLevelClass> GameLevels { get; private set; }
        public List<PlayerPowerClass> Powers { get; private set; }
        private GameService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                GameTypes = context.GameType.ToList();
                GameLevels = context.GameLevel.ToList();
                Powers = context.PlayerPower.ToList();
            }
        }
        public static GameService GetInstance(IServiceProvider serviceProvider)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameService(serviceProvider);
                    }
                }
            }
            return _instance;
        }

        private AppDbContext GetContext()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task<GameClass?> GetGame(int gameId)
        {
            using var context = GetContext();
            return await context.Games
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async ValueTask AddGameAsync(GameClass game)
        {
            using var context = GetContext();
            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();
        }

        public async ValueTask AddPlayerAsync(GameClass game, PlayerClass player)
        {
            using var context = GetContext();

            // Reattach game to new context if it's detached
            context.Attach(game);

            game.Players.Add(player);
            await context.SaveChangesAsync();

            var powers = await context.PlayerPower.ToListAsync();
            foreach (var power in powers)
            {
                PlayerPowerUseClass playerPowerUse = new PlayerPowerUseClass
                {
                    PlayerId = player.Id,
                    PlayerPowerId = power.Id,
                    IsUsed = false
                };
                await context.PlayerPowerUses.AddAsync(playerPowerUse);
            }
            await context.SaveChangesAsync();
        }

        public async Task CountdownTimer(GameClass game, GameTimerService _gameTimerService,
            IHubContext<GameHub> _hubContext, ObserverController _observerController)
        {
            using var context = GetContext();

            int countDown = ConstantService.StartGameCountdownSeconds;

            if (game != null)
            {
                context.Attach(game);
                game.IsOpen = false;
                await context.SaveChangesAsync();

                while (countDown >= 0)
                {
                    await _hubContext.Clients.Group(game.Id.ToString())
                        .SendAsync("timerClient", new { countDown, msg = "Starting game" });
                    await Task.Delay(1000);
                    countDown--;
                }

                _observerController.Notify();
                _ = _gameTimerService.StartGameTimer(game);
            }
        }

        public async Task<List<PlayerPowerUseRelation>> GetPlayerPowers(int playerId)
        {
            using var context = GetContext();
            return await context.PlayerPowerUses
                .Where(g => g.PlayerId == playerId)
                .Include(g => g.PlayerPower)
                .Select(g => new PlayerPowerUseRelation
                {
                    Id = g.Id,
                    PlayerPowerKey = g.PlayerPower.PlayerPowerKey,
                    ImagePath = g.PlayerPower.ImagePath,
                    IsUsed = g.IsUsed
                })
                .ToListAsync();
        }

        public async Task<PlayerClass?> GetPlayer(int playerId)
        {
            using var context = GetContext();
            return await context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);
        }

        public async Task<PlayerClass?> GetPlayerBySocketId(string socketId)
        {
            using var context = GetContext();
            return await context.Players
                .FirstOrDefaultAsync(p => p.SocketID == socketId);
        }

        public async Task RemovePlayer(PlayerClass player)
        {
            using var context = GetContext();
            context.Attach(player);
            context.Players.Remove(player);
            await context.SaveChangesAsync();
        }
    }
}
