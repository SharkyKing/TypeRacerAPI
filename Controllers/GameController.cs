using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeRacerAPI.Services;
using TypeRacerAPI.Controllers.ControllerHelperClasses;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Decorator;

namespace TypeRacerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly AppDbContext _context;

        public GameController(
            GameService gameService,
            IHubContext<GameHub> hubContext,
            AppDbContext context)
        {
            _gameService = gameService;
            _hubContext = hubContext;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameClass>> GetGame(int id)
        {
            var game = await _gameService.GetGame(id);
            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameClass>>> GetAllGames()
        {
            var games = await _context.Games
                .Include(g => g.Players)
                .ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}/players")]
        public async Task<ActionResult<IEnumerable<PlayerClass>>> GetPlayersInGame(int id)
        {
            var game = await _gameService.GetGame(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game.Players);
        }

        [HttpGet("levels")]
        public ActionResult<IEnumerable<GameTypeClass>> GetGameLevels()
        {
            return Ok(_gameService.GameLevels);
        }

        [HttpGet("types")]
        public ActionResult<IEnumerable<GameLevelClass>> GetGameTypes()
        {
            return Ok(_gameService.GameTypes);
        }

        [HttpGet("powers")]
        public ActionResult<IEnumerable<PlayerPowerClass>> GetGamePowers()
        {
            return Ok(_gameService.Powers);
        }
        [HttpGet("wordStyles")]
		public ActionResult<IEnumerable<string>> GetWordStyles()
		{
			var decoratedWords = new List<string>();

			foreach (var wordStyle in _gameService.WordStyles)
			{
				var word = new WordStyleDecorator(wordStyle.StyleName);

				if (!string.IsNullOrEmpty(wordStyle.fontFamily))
				{
					word = new FontFamilyDecorator(word, wordStyle.fontFamily);
				}

				if (!string.IsNullOrEmpty(wordStyle.fontWeight))
				{
					word = new FontWeightDecorator(word, wordStyle.fontWeight);
				}

				if (!string.IsNullOrEmpty(wordStyle.fontStyle))
				{
					word = new FontStyleDecorator(word, wordStyle.fontStyle);
				}

				decoratedWords.Add(word.GetStyledText());
			}

			return Ok(decoratedWords);
		}

		[HttpGet("player/{id}/powers")]
        public async Task<ActionResult<IEnumerable<PlayerPowerUseRelation>>> GetPlayerPowers(int id)
        {
            var playerPowers = await _gameService.GetPlayerPowers(id);
            return Ok(playerPowers);
        }
    }
}