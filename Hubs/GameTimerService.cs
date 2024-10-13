﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;
using Microsoft.EntityFrameworkCore;

public class GameTimerService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public GameTimerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // You can implement startup logic here if needed
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        // Clean-up logic can be implemented here if needed
    }

    public async Task StartGameTimer(int gameId, IHubContext<GameHub> hubContext)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var game = await context.Games
                .Include(g => g.Players)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                Console.WriteLine($"Game with ID {gameId} not found.");
                return;
            }

            game.StartTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            await context.SaveChangesAsync();

            int time = 3 * 60; // 3 minutes countdown

            while (time >= 0)
            {
                await hubContext.Clients.Group(gameId.ToString()).SendAsync("timerClient", new { countDown = time, msg = "" });
                await Task.Delay(1000); // Wait for 1 second
                time--;
            }
        }
    }
}
