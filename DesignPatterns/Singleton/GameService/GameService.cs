using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Controllers.ControllerHelperClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.DesignPatterns.Singleton.GameService
{
    public class GameService
    {
        #region PROPERTIES
        private readonly IServiceProvider _serviceProvider;
        private AppDbContext _context;
        #endregion
        #region INSTANCE CONTROL
        private static GameService? _instance;
        private static readonly object _lock = new object();
        #endregion
        #region DATA OBJECTS
        public List<GameTypeClass> GameTypes { get; private set; }
        public List<GameLevelClass> GameLevels { get; private set; }
        public List<PlayerPowerClass> Powers { get; private set; }
        #endregion
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
        public async Task CountdownTimer(GameClass game, GameTimerService _gameTimerService, IHubContext<GameHub> _hubContext)
        {
            _ = _gameTimerService.StartInitiatingGame(game, _serviceProvider);
        }
        #region GAME DATA CONTROL
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

            context.Attach(game);

            game.Players.Add(player);
            await context.SaveChangesAsync();

            MessageSystemBridge messageSystemBridge = new MessageSystemBridge(_serviceProvider, game.Id, player.Id ?? 0);
            await messageSystemBridge.SendMessageToGame("Joined this game");

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
        public async Task RemovePlayer(int? playerId)
        {
            using var context = GetContext();
            PlayerClass player = await context.Players.SingleOrDefaultAsync(player => player.Id == playerId);
            context.Attach(player);
            MessageSystemBridge messageSystemBridge = new MessageSystemBridge(_serviceProvider, player.GameId, player.Id ?? 0);
            await messageSystemBridge.SendMessageToGame("Disconnected");
            player.GameId = null;
            player.IsConnected = false;
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
