using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge;
using TypeRacerAPI.DesignPatterns.Strategy;
using TypeRacerAPI.DesignPatterns.Strategy.PowerStrategies;
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

                Regex regex = new Regex("[\\/][\\/][RFI][\\/]\\d+");

                if (regex.IsMatch(powerCast))
                {
                    string powerType = powerCast[2].ToString().ToLower();
                    bool attackPlayerIDParseSuccess = int.TryParse(powerCast.Substring(4, powerCast.Length - 4).ToString().ToLower(), out int victimPlayerId);

                    if (!attackPlayerIDParseSuccess) return false;

                    PowerController powerController = new PowerController(serviceProvider, playerId, victimPlayerId);

                    switch (powerType)
                    {
                        case "f":
                            powerController.SetPowerStrategy(new FreezePower());
                            break;
                        case "r":
                            powerController.SetPowerStrategy(new RewindPower());
                            break;
                        case "i":
                            powerController.SetPowerStrategy(new InvisiblePower());
                            break;
                    }

                    await powerController.Execute();

                    return true;
                }
                return false;
            }
        }
    }
}
