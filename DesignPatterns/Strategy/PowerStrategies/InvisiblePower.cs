﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Bridge;
using TypeRacerAPI.DesignPatterns.Strategy.Interface;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;
using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.DesignPatterns.Strategy.PowerStrategies
{
    public class InvisiblePower : IPowerStrategy
    {
        public string powerType { get; set; } = "I";
        public async ValueTask Attack(int? playerId, int? victimId, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();

                PlayerClass attackingPlayer = await _appDbContext.Players.SingleOrDefaultAsync(p => p.Id == playerId);
                PlayerClass victimPlayer = await _appDbContext.Players.SingleOrDefaultAsync(p => p.Id == victimId);

                PlayerPowerClass? playerPowerCasted =
                                await _appDbContext.PlayerPower.FirstOrDefaultAsync(power => power.PlayerPowerKey.ToLower() == powerType.ToLower());

                PlayerPowerUseClass? playerPowerUse =
                            await _appDbContext
                            .PlayerPowerUses
                            .FirstOrDefaultAsync(ppu =>
                                ppu.PlayerPowerId == playerPowerCasted.Id &&
                                ppu.PlayerId == playerId);

                PlayerPowerUseClass? playerPowerUseVictim =
                                await _appDbContext
                                .PlayerPowerUses
                                .FirstOrDefaultAsync(ppu =>
                                    ppu.PlayerPowerId == playerPowerCasted.Id &&
                                    ppu.PlayerId == victimPlayer.Id);

                if (!playerPowerUse.IsOnCooldown && !playerPowerUse.IsUsed)
                {
                    playerPowerUse.IsUsed = true;
                    playerPowerUseVictim.IsReceived = true;

                    MessageSystemBridge messageSystemBridge = new MessageSystemBridge(serviceProvider, attackingPlayer.GameId, attackingPlayer.Id);
                    await messageSystemBridge.SendMessageToGame("Used power [" + playerPowerCasted.PlayerPowerName + "] on player " + victimPlayer.NickName);

                    victimPlayer.WordVisible = false;
                    playerPowerUseVictim.IsReceived = true;
                    playerPowerUse.IsUsed = true;

                    await _appDbContext.SaveChangesAsync();
                    var game = await _appDbContext.Games
                      .Include(g => g.Players)
                      .SingleOrDefaultAsync(g => g.Id == attackingPlayer.GameId);

                    await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);

                    if (playerPowerCasted.IsTimedPower)
                    {
                        int time = ConstantService.TimedPowerSeconds;
                        await Task.Delay(time * 1000);

                        victimPlayer.WordVisible = true;

                        await _appDbContext.SaveChangesAsync();
                        game = await _appDbContext.Games
                      .Include(g => g.Players)
                      .SingleOrDefaultAsync(g => g.Id == attackingPlayer.GameId);

                        await _hubContext.Clients.Group(game.Id.ToString()).SendAsync(ConstantService.HubCalls[HubCall.UpdateGame], game);
                    }
                }
                else
                {
                    return;
                }
            }
            
        }
    }
}