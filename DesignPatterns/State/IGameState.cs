using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.State
{
    public interface IGameState
    {
        void Handle(GameClass game, IServiceScopeFactory serviceProvider);
    }
}
