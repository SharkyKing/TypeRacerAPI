using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.Services.Interface
{
    public interface IGameTimerService
    {
        Task StartInitiatingGame(GameClass game, IServiceProvider serviceProvider);
        Task StartGameTimer(int gameId, IServiceProvider serviceProvider);
        Task PowerCoolDownTimer(int? playerId, string connectionId, IServiceProvider _serviceProvider);
    }
}
