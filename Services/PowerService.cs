using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge;
using TypeRacerAPI.DesignPatterns.Bridge.LogBridges;
using TypeRacerAPI.DesignPatterns.Interpretator;
using TypeRacerAPI.DesignPatterns.Strategy;
using TypeRacerAPI.DesignPatterns.Strategy.PowerStrategies;
using TypeRacerAPI.DesignPatterns.Visitor;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI.Services
{
    public class PowerService
    {
        public async ValueTask<bool> TryAttack(string powerCast, int? playerId, int gameId, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                PowerCastExpression powerCastExpression = GetPowerCastExpression(powerCast);

                if (powerCastExpression == null)
                {
                    return false; 
                }

                int victimPlayerId;

                if (!powerCastExpression.Interpret(playerId, out victimPlayerId, powerCast))
                {
                    return false; 
                }

                PowerController powerController = new PowerController(serviceProvider, playerId, victimPlayerId);

                switch (powerCast[2]) 
                {
                    case 'f': 
                        powerController.SetPowerStrategy(new FreezePower());
                        break;
                    case 'r': 
                        powerController.SetPowerStrategy(new RewindPower());
                        break;
                    case 'i': 
                        powerController.SetPowerStrategy(new InvisiblePower());
                        break;
                    default:
                        return false; 
                }

                var visitor = new EntityLoggingVisitor(new LogGame(), serviceProvider);

                try
                {

                    (powerController.powerStrategy as IEntity).Accept(visitor);
                }
                catch (Exception ex)
                {

                }
                await powerController.Execute();

                return true;
            }
        }

        private PowerCastExpression GetPowerCastExpression(string powerCast)
        {
            switch (powerCast[2]) 
            {
                case 'f':
                    return new FreezePowerCast();
                case 'r':
                    return new RewindPowerCast();
                case 'i':
                    return new InvisiblePowerCast();
                default:
                    return null; 
            }
        }
    }
}
