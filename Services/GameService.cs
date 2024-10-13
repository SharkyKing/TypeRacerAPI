using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.Models;

namespace TypeRacerAPI.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Game> CreateGame(string words, string nickName, string socketId)
        {
            var game = new Game { Words = words };
            game.Players = new List<Player>();

            var player = new Player
            {
                SocketID = socketId,
                NickName = nickName,
                IsPartyLeader = true,
                Game = game
            };

            game.Players.Add(player);
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game> JoinGame(int gameId, string nickName, string socketId)
        {
            var game = await _context.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == gameId);

            var player = new Player
            {
                SocketID = socketId,
                NickName = nickName,
                IsPartyLeader = false,
                Game = game
            };

            game.Players.Add(player);
            await _context.SaveChangesAsync();
            return game;
        }
    }
}
