using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.Services
{
    public class LogService
    {
        IServiceProvider serviceProvider;
        int? gameId = null;
        int? playerId = null;
        public LogService(IServiceProvider serviceProvider, int? gameId, int? playerId)
        {
            this.serviceProvider = serviceProvider;
            this.gameId = gameId;
            this.playerId = playerId;
        }
        public LogService(IServiceProvider serviceProvider, int gameId)
        {
            this.serviceProvider = serviceProvider;
            this.gameId = gameId;
        }
        public LogService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private async ValueTask LogMessage(string message, int logTypeId)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await _appDbContext.GameLog.AddAsync(new GameLogClass()
                {
                    Message = message,
                    GameId = gameId,
                    PlayerId = playerId,
                    DateCreated = DateTime.UtcNow,
                    LogTypeId = logTypeId
                });

                await _appDbContext.SaveChangesAsync();
            }
        }

        public async ValueTask LogError(string message)
        {
            await LogMessage(message, 1);
        }

        public async ValueTask LogGame(string message)
        {
            await LogMessage(message, 2);
        }

        public async ValueTask LogInformation(string message)
        {
            await LogMessage(message, 3);
        }
    }
}
