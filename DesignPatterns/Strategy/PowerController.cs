using TypeRacerAPI.DesignPatterns.Strategy.Interface;
using TypeRacerAPI.DesignPatterns.Visitor;

namespace TypeRacerAPI.DesignPatterns.Strategy
{
    public class PowerController
    {
        public IPowerStrategy powerStrategy;
        public IEntity powerEntity;
        IServiceScopeFactory serviceProvider;
        int? playerId;
        int? victimId;

        public PowerController(IServiceScopeFactory serviceProvider, int? playerId, int? victimId)
        {
            this.serviceProvider = serviceProvider;
            this.playerId = playerId;
            this.victimId = victimId;
        }

        public void SetPowerStrategy(IPowerStrategy _powerStrategy, IEntity _powerEntity)
        {
            this.powerStrategy = _powerStrategy;   
            this.powerEntity = _powerEntity;   
        }

        public async ValueTask Execute()
        {
            await powerStrategy.Attack(playerId, victimId, serviceProvider);
        }
    }
}
