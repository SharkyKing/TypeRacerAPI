
using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Interface;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;
using TypeRacerAPI.DesignPatterns.Facade.Interface;
using TypeRacerAPI.DesignPatterns.Factory.Player;
using TypeRacerAPI.DesignPatterns.Factory.Player.Enum;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.DesignPatterns.Visitor;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.DesignPatterns.Facade
{
    public class GameCreateFacade : IGameFacade
    {
        public Singleton.GameService.GameService gameService { get; set; }
        public PlayerFactory playerFactory { get; set; }
        public AppDbContext appDbContext { get; set; }
        public IHubContext<GameHub> _hubContext { get; set; }
        public ObserverController _observerController { get; set; }

        public GameCreateFacade(AppDbContext context, IHubContext<GameHub> hubContext, ObserverController observerController, GameService _gameService)
        {
            appDbContext = context;
            _hubContext = hubContext;
            _observerController = observerController;
            gameService = _gameService;
        }

        public async ValueTask<GameClass?> Execute(string nickName, string connectionGUID, int activeGameType, int activeGameLevel, int gameId)
        {
            GameClass? game = new GameClass();
            PlayerClass player = new PlayerClass();

            GameLevelClass? gameLevel = gameService.GameLevels.Where(gl => gl.Id == activeGameLevel).FirstOrDefault();
            GameTypeClass? gameType = gameService.GameTypes.Where(gt => gt.Id == activeGameType).FirstOrDefault();

            if(gameLevel != null && gameType != null)
            {
                game = await GameCreator.CreateGameObj(activeGameType, activeGameLevel, appDbContext);
            }

            if (game != null)
            {
                playerFactory = new PlayerFactory(game.Id, nickName, connectionGUID);
                player = playerFactory.CreatePlayer(PlayerType.Leader);
                await gameService.AddPlayerAsync(game, player);
            }
            else return null;

            return game;
        }
    }
}
