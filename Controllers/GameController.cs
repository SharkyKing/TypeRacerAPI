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
using TypeRacerAPI.DesignPatterns.Command;
using TypeRacerAPI.DesignPatterns.Iterator;

namespace TypeRacerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _serviceProvider;

        public GameController(
            GameService gameService,
            IHubContext<GameHub> hubContext,
            AppDbContext context,
            IServiceScopeFactory serviceProvider)
        {
            _gameService = gameService;
            _hubContext = hubContext;
            _context = context;
            _serviceProvider = serviceProvider;
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
			CustomIterableCollection<WordsStyleClass> wordsStyleClasses = new CustomIterableCollection<WordsStyleClass>();
			var database = new DatabaseReceiver();
			try
			{
				var invoker = new DatabaseInvoker();
				var selectCommand = new SelectCommand(database, "WordsStyle");
				invoker.AddCommand(selectCommand);
				invoker.ExecuteCommands();
				wordsStyleClasses = new WordsStyleClass().Filler(database._results);

				var deleteCommand = new DeleteCommand(database, 0, "WordsStyle");
				var updateCommand = new UpdateCommand(database, "WordsStyle", 0, "test", "");
				invoker.AddCommand(deleteCommand);
				invoker.AddCommand(updateCommand);
				//invoker.ExecuteCommands();
			}
			catch
			{
			}

			if (database._results != null)
			{
				var iterator = wordsStyleClasses.CreateIterator();
				while (iterator.HasNext())
				{
					var wordStyle = iterator.Next();
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
			}
			else
			{
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
			}

			return Ok(decoratedWords);
		}



		[HttpGet("playerGameResults")]
        public ActionResult<IEnumerable<PlayerPowerClass>> GetPlayerGameResults()
        {
            return Ok(_gameService.PlayerGameResults);
        }
        [HttpGet("player/{id}/powers")]
        public async Task<ActionResult<IEnumerable<PlayerPowerUseRelation>>> GetPlayerPowers(int id)
        {
            var playerPowers = await _gameService.GetPlayerPowers(id);
            return Ok(playerPowers);
        }

        [HttpGet("lastMessage/{id}")]
        public async Task<ActionResult<string>> GetLastMessage(int id)
        {
            var lastMessage = GameService.GetInstance(_serviceProvider).GetLastSavedMessage(id)?.Message;

            if (lastMessage == null)
            {
                return Ok("");
            }

            return Ok(lastMessage); 
        }
    }
}