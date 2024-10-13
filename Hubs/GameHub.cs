using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TypeRacerAPI.Data;
using TypeRacerAPI.Models;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Services;
using System.Timers;

namespace TypeRacerAPI.Hubs
{
    public class GameHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly GameTimerService _gameTimerService;

        public GameHub(AppDbContext context, IHubContext<GameHub> hubContext, GameTimerService gameTimerService)
        {
            _context = context;
            _hubContext = hubContext;
            _gameTimerService = gameTimerService;
        }

        public async Task CreateGame(string nickName)
        {
            GameService gameService = new GameService(_context);
            Game game = await gameService.CreateGame("Čia gan įdomus tekstas apie žodžius", nickName, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());
            await Clients.Group(game.Id.ToString()).SendAsync("UpdateGame", game);
        }

        public async Task JoinGame(string gameId,string nickName)
        {
            GameService gameService = new GameService(_context);
            Game game = await gameService.JoinGame(int.Parse(gameId), nickName, Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id.ToString());
            await Clients.Group(game.Id.ToString()).SendAsync("UpdateGame", game);
        }

        public async Task StartTimer(int playerId, int gameId)
        {
            try
            {
                int countDown = 5;

                var game = await _context.Games
                    .Include(g => g.Players)
                    .FirstOrDefaultAsync(g => g.Id == gameId);

                if (game == null)
                {
                    return;
                }

                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return;
                }

                if (player.IsPartyLeader)
                {
                    await CountdownTimer(gameId, countDown);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling timer event: " + ex.Message);
            }
        }

        private async Task CountdownTimer(int gameId, int countDown)
        {
            while (countDown >= 0)
            {
                await Clients.Group(gameId.ToString()).SendAsync("timerClient", new { countDown, msg = "Starting game" });
                await Task.Delay(1000); 
                countDown--;
            }

            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                game.IsOpen = false;
                await _context.SaveChangesAsync();

                await Clients.Group(game.Id.ToString()).SendAsync("UpdateGame", game);
                await StartGameClock(gameId);
            }
        }

        private async Task StartGameClock(int gameId)
        {
            _gameTimerService.StartGameTimer(gameId, _hubContext);
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
                        var words = game.Words.Split(" ");
                        string word = words[player.CurrentWordIndex];

                        if (word == userInput)
                        {
                            player.CurrentWordIndex++;

                            if (player.CurrentWordIndex < words.Length)
                            {
                                await _context.SaveChangesAsync();
                                await Clients.Group(gameId.ToString()).SendAsync("UpdateGame", game);
                            }
                            else
                            {
                                DateTime endTime = DateTime.UtcNow; 
                                DateTime startTime = new DateTime(game.StartTime * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                                player.WPM = CalculateWPM(endTime, startTime, player);

                                await _context.SaveChangesAsync();
                                await Clients.Group(gameId.ToString()).SendAsync("UpdateGame", game);
                                await Clients.Group(gameId.ToString()).SendAsync("done", new { game, playerWon = player });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling user input: " + ex.Message);
            }
        }

        private string CalculateTime(int seconds)
        {
            var minutes = seconds / 60;
            var remainingSeconds = seconds % 60;
            return $"{minutes:D2}:{remainingSeconds:D2}"; 
        }

        private int CalculateWPM(DateTime endTime, DateTime startTime, Player player)
        {
            var totalTimeInMinutes = (endTime - startTime).TotalMinutes;
            var wordsTyped = player.CurrentWordIndex; 
            return (int)(wordsTyped / totalTimeInMinutes);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine($"Client connected with ID: {connectionId}");

            await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);

            await base.OnConnectedAsync();
        }
    }
}
