using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.ArchitectureTemplates.PowerTemplate
{
    public class PowerController
    {
        public async ValueTask<bool> IdentifyPower(string powerCast, AppDbContext _context)
        {
            if (powerCast.Contains("//"))
            {
                try
                {
                    if (powerCast.Length == 3)
                    {
                        string powerType = powerCast[2].ToString().ToLower();

                        PlayerPowerBase playerPowerCasted = 
                            await _context.PlayerPower.FirstOrDefaultAsync(power => power.PlayerPowerKey.ToLower() == powerType);

                        if(playerPowerCasted != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
