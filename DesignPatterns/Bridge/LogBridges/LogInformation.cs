using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.DesignPatterns.Bridge.LogBridges
{
    public class LogInformation : ILogService
    {
        public async ValueTask Log(string message, int? gameId, int? playerId, IServiceProvider serviceProvider)
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
                    LogTypeId = 3
                });

                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
