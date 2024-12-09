using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;
using TypeRacerAPI.DesignPatterns.Bridge;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;
using static TypeRacerAPI.EnumClass;
using TypeRacerAPI.DesignPatterns.Chain;

namespace TypeRacerAPI.Services
{
    public static class UserInputService
    {
        public class UserInputServiceCheckResult
        {
            public bool powerUsed = false;
        }
        public static async ValueTask<UserInputServiceCheckResult> CheckUserInput(int? playerId, int gameId, string userInput, IServiceScopeFactory serviceProvider)
        {
            UserInputServiceCheckResult userInputServiceCheckResult = new UserInputServiceCheckResult();

            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                var game = await _appDbContext.Games
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == gameId);

                if (game != null && !game.IsOpen && !game.IsOver)
                {
                    var player = game.Players.FirstOrDefault(p => p.Id == playerId);

                    if (player != null)
                    {
                        PowerService powerController = new PowerService();

                        userInputServiceCheckResult.powerUsed = await powerController.TryAttack(userInput, player.Id, game.Id, serviceProvider);

                        if (userInputServiceCheckResult.powerUsed)
                        {
                            return userInputServiceCheckResult;
                        }

                        var words = game.Words.Split(" ");
                        string word = words[player.CurrentWordIndex];

                        if (word == userInput)
                        {
                            player.CurrentWordIndex++;
                            _appDbContext.Players.Attach(player);

                            if (player.CurrentWordIndex < words.Length)
                            {
                                await _appDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                DateTime endTime = DateTime.UtcNow;
                                DateTime startTime = new DateTime(game.StartTime * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                                player.WPM = LogicHelper.CalculateWPM(endTime, startTime, player);
                                player.Finished = true;

                                game.IsOver = true;

                                if (game.GameTypeId == 2)
                                {
                                    player.InputEnabled = false;

                                    MessageSystemBridge messageSystemBridge = new MessageSystemBridge(new LogGame(), serviceProvider, gameId, player.Id);
                                    await messageSystemBridge.SendMessageToGame("Finished typing. Waiting for others..");

                                    await _appDbContext.SaveChangesAsync();

                                    IEndGameHandler handler1 = new CheckAllPlayersFinishedHandler();
                                    IEndGameHandler handler2 = new DetermineWinnerHandler();
                                    IEndGameHandler handler3 = new SendGameOverMessageHandler();

                                    handler1.SetNextHandler(handler2);
                                    handler2.SetNextHandler(handler3);

                                    await handler1.HandleEndGameAsync(game, player, serviceProvider, _hubContext);

                                    //List<PlayerClass> players = await _appDbContext.Players.Where(p => p.GameId == game.Id).ToListAsync();

                                    //bool allFinished = true;
                                    //PlayerClass playerWithLeastMistakes = null;
                                    //int leastMistakes = int.MinValue;

                                    //foreach (PlayerClass playerUnit in players)
                                    //{
                                    //    if (!player.Finished)
                                    //    {
                                    //        allFinished = false;
                                    //        break;
                                    //    }

                                    //    if (playerUnit.MistakeCount > leastMistakes && playerUnit.Finished)
                                    //    {
                                    //        playerWithLeastMistakes = playerUnit;
                                    //    }
                                    //}

                                    //if (allFinished)
                                    //{
                                    //    game.IsOver = true;
                                    //    await _appDbContext.SaveChangesAsync();
                                    //    await _hubContext.Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = playerWithLeastMistakes });
                                    //}

                                }
                                else
                                {
                                    game.IsOver = true;
                                    await _appDbContext.SaveChangesAsync();
                                    await _hubContext.Clients.Group(gameId.ToString()).SendAsync(ConstantService.HubCalls[HubCall.Done], new { playerWon = player });
                                }
                            }
                        }
                        else
                        {
                            player.MistakeCount++;
                            await _appDbContext.SaveChangesAsync();
                        }
                    }
                }

                return userInputServiceCheckResult;
            }
        }
    }
}
