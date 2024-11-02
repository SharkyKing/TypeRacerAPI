using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<GameBase>> GetGame(int id)
        {
            var game = await _context.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameBase>>> GetAllGames()
        {
            var games = await _context.Games.Include(g => g.Players).ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}/players")]
        public async Task<ActionResult<IEnumerable<PlayerBase>>> GetPlayersInGame(int id)
        {
            var game = await _context.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game.Players);
        }

        [HttpGet("levels")]
        public async Task<ActionResult<IEnumerable<GameTypeBase>>> GetGameLevels()
        {
            var gameLevels = await _context.GameLevel.ToListAsync();
            return Ok(gameLevels);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<GameLevelBase>>> GetGameTypes()
        {
            var gameTypes = await _context.GameType.ToListAsync();
            return Ok(gameTypes);
        }

        [HttpGet("powers")]
        public async Task<ActionResult<IEnumerable<GameLevelBase>>> GetGamePowers()
        {
            var playerPowers = await _context.PlayerPower.ToListAsync();
            return Ok(playerPowers);
        }
    }
}
