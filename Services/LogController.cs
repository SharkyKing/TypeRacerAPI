using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;

namespace TypeRacerAPI.Services
{
    public class LogController
    {
        IServiceProvider serviceProvider;
        ILogService logService;
        int? gameId = null;
        int? playerId = null;
        public LogController(ILogService logService, IServiceProvider serviceProvider, int? gameId, int? playerId)
        {
            this.logService = logService;
            this.serviceProvider = serviceProvider;
            this.gameId = gameId;
            this.playerId = playerId;
        }
        public LogController(ILogService logService, IServiceProvider serviceProvider, int gameId)
        {
            this.logService = logService;
            this.serviceProvider = serviceProvider;
            this.gameId = gameId;
        }
        public LogController(ILogService logService, IServiceProvider serviceProvider)
        {
            this.logService = logService;
            this.serviceProvider = serviceProvider;
        }

        public async ValueTask LogMessage(string message)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                await logService.Log(message, gameId, playerId, serviceProvider);
            }
        }
    }
}
