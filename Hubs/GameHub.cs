using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TypeRacerAPI.Data;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Services;
using System.Timers;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.DesignPatterns.Facade.Interface;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Observer;
using static TypeRacerAPI.EnumClass;
using TypeRacerAPI.DesignPatterns.Bridge;

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

                GameObserver gameObserver = new GameObserver();
                gameObserver.SetGameId(game.Id);

                _observerController.Attach(gameObserver);
                await gameObserver.Update(_serviceProvider);
            }
        }
        public async Task JoinGame(string gameId, string nickName)
        {
            IGameFacade gameJoinFacade = new GameJoinFacade(_context, _hubContext, _observerController, _gameService);

            GameClass game = await gameJoinFacade.Execute(nickName, Context.ConnectionId, 0, 0, int.Parse(gameId));

            if (game != null)
            {
                await AddMeToGroup(game.Id.ToString());
                await _observerController.Notify(_serviceProvider);
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
                    await _gameService.CountdownTimer(game, _gameTimerService, _hubContext);
                }
            }
        }
        public async Task UserInput(string userInput, int gameId)
        {
            try
            {
                var game = await _context.Games
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == gameId);

                if (game != null && !game.IsOpen && !game.IsOver)
                {
                    var player = game.Players.FirstOrDefault(p => p.SocketID == Context.ConnectionId);

                    if (player != null)
                    {
                        PowerService powerController = new PowerService();

                        bool powerUsed = await powerController.TryAttack(userInput, player.Id, game.Id, _serviceProvider);

                        if (powerUsed)
                        {
                            _ = _observerController.Notify(_serviceProvider);
                            _ = _gameTimerService.PowerCoolDownTimer(player.Id, Context.ConnectionId, _serviceProvider);
                            return;
                        }

                        var words = game.Words.Split(" ");
                        string word = words[player.CurrentWordIndex];

                        if (word == userInput)
                        {
                            player.CurrentWordIndex++;

                            if (player.CurrentWordIndex < words.Length)
                            {
                                await _context.SaveChangesAsync();
                                await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
                            }
                            else
                            {
                                DateTime endTime = DateTime.UtcNow;
                                DateTime startTime = new DateTime(game.StartTime * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                                player.WPM = LogicHelper.CalculateWPM(endTime, startTime, player);
                                player.Finished = true;

                                if(game.GameTypeId == 2)
                                {
                                    player.InputEnabled = false;
                                    await _context.SaveChangesAsync();
                                    MessageSystemBridge messageSystemBridge = new MessageSystemBridge(_serviceProvider, gameId, player.Id);
                                    await messageSystemBridge.SendMessageToGame("finished typing. Waiting for others..");

                                    List<PlayerClass> players = await _context.Players.Where(p => p.GameId == game.Id).ToListAsync();

                                    bool allFinished = true;
                                    PlayerClass playerWithLeastMistakes = null;
                                    int leastMistakes = int.MinValue;

                                    foreach (PlayerClass playerUnit in players)
                                    {
                                        if (!player.Finished)
                                        {
                                            allFinished = false;
                                            break;
                                        }

                                        if(playerUnit.MistakeCount > leastMistakes && playerUnit.Finished)
                                        {
                                            playerWithLeastMistakes = playerUnit;
                                        }
                                    }

                                    if (allFinished)
                                    {
                                        game.IsOver = true;
                                        await _context.SaveChangesAsync();
                                        await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
                                        await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = playerWithLeastMistakes });
                                    }

                                }
                                else
                                {
                                    game.IsOver = true;
                                    await _context.SaveChangesAsync();
                                    await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
                                    await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = player });
                                }
                            }
                        }
                        else
                        {
                            player.MistakeCount++;
                            await _context.SaveChangesAsync();
                            await Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
                        }
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
            _ = _gameTimerService.PowerCoolDownTimer(id, Context.ConnectionId, _serviceProvider);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var playerDisconnecting = await _gameService.GetPlayerBySocketId(Context.ConnectionId);

            if (playerDisconnecting != null)
            {
                await _gameService.RemovePlayer(playerDisconnecting.Id);
                _ = _observerController.Notify(_serviceProvider);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected with ID: {connectionId}");

            await Clients.Caller.SendAsync(ConstantService.HubCalls[HubCall.ReceiveConnectionId], connectionId);

            await base.OnConnectedAsync();
        }
        public async ValueTask AddMeToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }
        public async Task SendMessage(int gameId, int playerId, string inputValue)
        {
            MessageSystemBridge messageSystemBridge = new MessageSystemBridge(_serviceProvider, gameId, playerId);
            await messageSystemBridge.SendMessageToGame(inputValue);
        }
    }
}