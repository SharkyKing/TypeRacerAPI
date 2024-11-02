using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.AccessControl;
using TypeRacerAPI.ArchitectureTemplates.AbstractFactory.Game;
using TypeRacerAPI.ArchitectureTemplates.FactoryMethod.Player;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using static TypeRacerAPI.ArchitectureTemplates.FactoryMethod.Player.PlayerFactories;

namespace TypeRacerAPI.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;
        private static GameService _instance;
        private static readonly object _lock = new object();
        public List<GameTypeBase> GameTypes { get;private set; }
        public List<GameLevelBase> GameLevels { get;private set; }
        public List<PlayerPowerBase> Powers { get;private set; }
        private GameService(AppDbContext context)
        {
            _context = context;

            GameTypes = _context.GameType.ToList();
            GameLevels = _context.GameLevel.ToList();
            Powers = _context.PlayerPower.ToList();
        }

        public static GameService GetInstance(AppDbContext context)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameService(context);
                    }
                }
            }
            return _instance;
        }

        public async Task<GameBase> CreateGame(string nickName, string socketId, int activeGameType, int activeGameLevel)
        {
            IGameFactory gameFactory = new GameFactory();

            GameLevelBase gameLevel = GameLevels.Where(gl => gl.Id == activeGameLevel).FirstOrDefault();
            GameTypeBase gameType = GameTypes.Where(gt => gt.Id == activeGameType).FirstOrDefault();

            GameBase game;

            switch (gameLevel.Id)
            {
                case 1:
                    game = gameFactory.CreateBeginnerGame();
                    break;
                case 2:
                    game = gameFactory.CreateNormalGame();
                    break;
                case 3:
                    game = gameFactory.CreateAdvancedGame();
                    break;
                default:
                    throw new NotImplementedException("That game type is not implemented");
            }

            game.GameLevel = gameLevel;
            game.GameType = gameType;

            IPlayerFactory leaderFactory = new PlayerLeaderFactory();
            PlayerBase leader = leaderFactory.CreatePlayer(game.Id, nickName, socketId);

            await AddGameAsync(game);
            await AddPlayerAsync(game, leader);
            return game;
        }

        public async Task<GameBase> JoinGame(int gameId, string nickName, string socketId)
        {
            var game = await _context.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == gameId);

            IPlayerFactory guestFactory = new PlayerGuestFactory();
            PlayerBase guest = guestFactory.CreatePlayer(game.Id, nickName, socketId);

            game.Players.Add(guest);
            await _context.SaveChangesAsync();
            return game;
        }

        public async ValueTask SaveAsync(GameBase game)
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask AddGameAsync(GameBase game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async ValueTask AddPlayerAsync(GameBase game, PlayerBase player)
        {
            game.Players.Add(player);

            await _context.SaveChangesAsync();

            var powers = await _context.PlayerPower.ToListAsync();

            foreach (var power in powers)
            {
                PlayerPowerUse playerPowerUse = new PlayerPowerUse
                {
                    PlayerId = player.Id,
                    PlayerPowerId = power.Id,
                    IsUsed = false 
                };

                await _context.PlayerPowerUses.AddAsync(playerPowerUse);
            }

            await _context.SaveChangesAsync();
        }
    }
}
