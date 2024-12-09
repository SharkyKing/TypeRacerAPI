using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;
using TypeRacerAPI.DesignPatterns.Strategy.PowerStrategies;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.DesignPatterns.Visitor
{
    public class EntityLoggingVisitor : IEntityVisitor
    {
        LogController logController;
        public EntityLoggingVisitor(ILogService logService, IServiceScopeFactory serviceProvider)
        {
            logController = new LogController(logService, serviceProvider);
        }

        public async ValueTask Visit(FreezePower power)
        {
            await logController.LogMessage(power.powerType + " was casted by player " + power.playerId + " onto victim player " + power.victimId);
        }

        public async ValueTask Visit(RewindPower power)
        {
            await logController.LogMessage(power.powerType + " was casted by player " + power.playerId + " onto victim player " + power.victimId);
        }

        public async ValueTask Visit(InvisiblePower power)
        {
            await logController.LogMessage(power.powerType + " was casted by player " + power.playerId + " onto victim player " + power.victimId);
        }
    }

}
