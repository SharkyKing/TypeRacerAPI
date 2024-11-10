using TypeRacerAPI.DesignPatterns.Strategy.Interface;

namespace TypeRacerAPI.DesignPatterns.Strategy
{
    public class PowerController
    {
        IPowerStrategy powerStrategy;
        IServiceProvider serviceProvider;
        int? playerId;
        int? victimId;

        public PowerController(IServiceProvider serviceProvider, int? playerId, int? victimId)
        {
            this.serviceProvider = serviceProvider;
            this.playerId = playerId;
            this.victimId = victimId;
        }

        public void SetPowerStrategy(IPowerStrategy _powerStrategy)
        {
            this.powerStrategy = _powerStrategy;   
        }

        public async ValueTask Execute()
        {
            await powerStrategy.Attack(playerId, victimId, serviceProvider);
        }
    }
}
