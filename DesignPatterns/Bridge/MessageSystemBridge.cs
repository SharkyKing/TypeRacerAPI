using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.Bridge
{
    public class MessageSystemBridge : IMessageSystemBridge
    {
        LogController logController;
        IServiceProvider _serviceProvider;
        int? gameId;
        int? playerId;
        public MessageSystemBridge(ILogService logService,IServiceProvider serviceProvider, int? gameId, int? playerId)
        {
            logController = new LogController(logService, serviceProvider, gameId, playerId);
            _serviceProvider = serviceProvider;
            this.gameId = gameId;
            this.playerId = playerId;
        }

        public async ValueTask SendMessageToGame(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                var player = await _appDbContext.Players
                        .SingleOrDefaultAsync(p => p.Id == playerId);

                await _hubContext.Clients.Group(gameId.ToString())
                    .SendAsync(ConstantService.HubCalls[HubCall.SendMessageToGame], new { playerNickName = player.NickName, msg = message });

                await logController.LogMessage(message);
            }
        }
    }
}
