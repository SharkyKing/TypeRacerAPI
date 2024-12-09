using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.DesignPatterns.Strategy.Interface
{
    public interface IPowerStrategy
    {
        string powerType { get; set; }
        ValueTask Attack(int? playerId, int? victimId, IServiceScopeFactory serviceProvider);
    }
}
