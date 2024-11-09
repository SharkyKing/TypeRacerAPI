using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TypeRacerAPI.Data;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Services;
using System.Timers;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.ArchitectureTemplates.PowerTemplate;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.DesignPatterns.Facade.Interface;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Observer;

namespace TypeRacerAPI.Hubs
{
    public class GameHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly GameTimerService _gameTimerService;
        private readonly GameService _gameService;
        private readonly ObserverController _observerController;
        private readonly IServiceProvider _serviceProvider;

        public GameHub(
            AppDbContext context,
            IHubContext<GameHub> hubContext,
            GameTimerService gameTimerService,
            GameService gameService,
            ObserverController observerController,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _hubContext = hubContext;
            _gameTimerService = gameTimerService;
            _gameService = gameService;
            _observerController = observerController;
            _serviceProvider = serviceProvider;
        }

        public async Task CreateGame(string nickName, int activeGameType, int activeGameLevel)
        {
            IGameFacade gameCreateFacade = new GameCreateFacade(_context, _hubContext, _observerController, _gameService);

            GameClass game = await gameCreateFacade.Execute(nickName, Context.ConnectionId, activeGameType, activeGameLevel, 0);

            if (game != null)
            {
                await AddMeToGroup(game.Id.ToString());

                GameObserver gameObserver = new GameObserver(_hubContext, _serviceProvider);
                gameObserver.SetGameId(game.Id);

                _observerController.Attach(gameObserver);
                _observerController.Notify();
            }
        }

        public async Task JoinGame(string gameId, string nickName)
        {
            IGameFacade gameJoinFacade = new GameJoinFacade(_context, _hubContext, _observerController, _gameService);

            GameClass game = await gameJoinFacade.Execute(nickName, Context.ConnectionId, 0, 0, 0);

            if (game != null)
            {
                await AddMeToGroup(game.Id.ToString());
                _observerController.Notify();
            }
        }

        public async Task StartTimer(int playerId, int gameId)
        {
            var game = await _gameService.GetGame(gameId);

            if (game != null)
            {
                var player = await _gameService.GetPlayer(playerId);
                if (player != null && player.IsPartyLeader)
                {
                    await _gameService.CountdownTimer(game, _gameTimerService, _hubContext, _observerController);
                }
            }
        }

        /* Uncomment and update these methods as needed
        public async Task UserInput(string userInput, int gameId)
        {
            try
            {
                var game = await _gameService.GetGame(gameId);

                if (game != null && !game.IsOpen && !game.IsOver)
                {
                    var player = game.Players.FirstOrDefault(p => p.SocketID == Context.ConnectionId);

                    if (player != null)
                    {
                        PowerController powerController = new PowerController();

                        bool powerUsed = await powerController.IdentifyPower(userInput, player, game.Id, _context);

                        if (powerUsed)
                        {
                            _observerController.Notify();
                            return;
                        }

                        // Rest of the game logic...
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling user input: " + ex.Message);
            }
        }

        public async Task StartPowerCooldown(int id)
        {
            _ = _gameTimerService.PowerCoolDownTimer(id, _hubContext, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var playerDisconnecting = await _gameService.GetPlayerBySocketId(Context.ConnectionId);

            if (playerDisconnecting != null)
            {
                await _gameService.RemovePlayer(playerDisconnecting);
                _observerController.Notify();
            }

            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected with ID: {connectionId}");

            await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);

            await base.OnConnectedAsync();
        }
        */

        public async ValueTask AddMeToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }
    }
}