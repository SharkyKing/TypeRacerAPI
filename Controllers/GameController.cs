using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Models;
using TypeRacerAPI.Data;
using TypeRacerAPI.Services;
using System.Text.RegularExpressions;

namespace TypeRacerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Game>> CreateGame([FromBody] string nickName)
        {
            GameService gameService = new GameService(_context);
            Game game = await gameService.CreateGame("TEST TEST", nickName, "TempSocketID" + (new Random()).Next(10000, 99999));

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }
        [HttpPost("join")]
        public async Task<ActionResult<Game>> JoinGame(int gameId, string nickName)
        {
            GameService gameService = new GameService(_context);
            Game game = await gameService.JoinGame(gameId, nickName, "TempSocketID" + (new Random()).Next(10000, 99999));

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }
    }
}
