using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Facade.Interface;
using TypeRacerAPI.DesignPatterns.Factory.Player.Enum;
using TypeRacerAPI.DesignPatterns.Factory.Player;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.DesignPatterns.Facade
{
    public class GameJoinFacade : IGameFacade
    {
        private readonly GameService _gameService;
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ObserverController _observerController;

        private PlayerFactory _playerFactory;

        public GameJoinFacade(AppDbContext context, IHubContext<GameHub> hubContext, ObserverController observerController, GameService gameService)
        {
            _appDbContext = context;
            _hubContext = hubContext;
            _observerController = observerController;
            _gameService = gameService;
        }

        public async ValueTask<GameClass> Execute(string nickName, string socketId, int activeGameType, int activeGameLevel, int gameId)
        {
            GameClass game = await _gameService.GetGame(gameId);

            if (game != null)
            {
                _playerFactory = new PlayerFactory(gameId, nickName, socketId);
                PlayerClass player = _playerFactory.CreatePlayer(PlayerType.Guest);

                await _gameService.AddPlayerAsync(game, player);
                return game;
            }

            return null;
        }
    }
}

