using Microsoft.EntityFrameworkCore;
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

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GameBase> CreateGame(string nickName, string socketId, int activeGameType, int activeGameLevel)
        {
            IGameFactory gameFactory = new GameFactory();

            GameLevelBase gameLevel = await _context.GameLevel.FirstOrDefaultAsync(gl => gl.Id == activeGameLevel);
            GameTypeBase gameType = await _context.GameType.FirstOrDefaultAsync(gt => gt.Id == activeGameType);

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
