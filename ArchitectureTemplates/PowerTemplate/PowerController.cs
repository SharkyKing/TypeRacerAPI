using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.ArchitectureTemplates.PowerTemplate
{
    public class PowerController
    {
        public async ValueTask<bool> IdentifyPower(string powerCast, PlayerClass player,int gameId, AppDbContext _context)
        {
            if (powerCast.StartsWith("//"))
            {
                try
                {
                    if (powerCast.Length == 5)
                    {
                        string powerType = powerCast[2].ToString().ToLower();
                        bool attackPlayerIDParseSuccess = int.TryParse(powerCast[4].ToString().ToLower(), out int attackPlayerId);

                        if (!attackPlayerIDParseSuccess) return false;

                        PlayerPowerClass? playerPowerCasted = 
                            await _context.PlayerPower.FirstOrDefaultAsync(power => power.PlayerPowerKey.ToLower() == powerType);

                        if (playerPowerCasted == null) return false;

                        PlayerClass? victimPlayer =
                            await _context
                                .Players
                                .FirstOrDefaultAsync(player => player.Id == attackPlayerId && player.GameId == gameId);

                        if (victimPlayer == null) return false;

                        PlayerPowerUseClass? playerPowerUse =
                            await _context
                            .PlayerPowerUses
                            .FirstOrDefaultAsync(ppu => 
                                ppu.PlayerPowerId == playerPowerCasted.Id && 
                                ppu.PlayerId == player.Id);

                        if (playerPowerUse == null) return false;

                        PlayerPowerUseClass? playerPowerUseVictim =
                            await _context
                            .PlayerPowerUses
                            .FirstOrDefaultAsync(ppu =>
                                ppu.PlayerPowerId == playerPowerCasted.Id &&
                                ppu.PlayerId == victimPlayer.Id);

                        if (playerPowerUse == null) return false;

                        if (!playerPowerUse.IsOnCooldown && !playerPowerUse.IsUsed)
                        {
                            return AttackPlayer(playerPowerUse, playerPowerUseVictim, victimPlayer);
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

        public bool AttackPlayer(PlayerPowerUseClass playerPower, PlayerPowerUseClass playerPowerVictim, PlayerClass victimPlayer)
        {
            playerPower.IsUsed = true;
            playerPowerVictim.IsReceived = true;

            return true;
        }
    }
}
