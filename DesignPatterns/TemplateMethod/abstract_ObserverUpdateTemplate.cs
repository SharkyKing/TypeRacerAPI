using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.DesignPatterns.TemplateMethod
{
    public abstract class ObserverUpdateTemplate
    {
        public async Task UpdateAsync(IServiceProvider serviceProvider, int id)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<Hubs.GameHub>>();

                var entity = await GetEntityAsync(appDbContext, id);

                if (entity == null)
                {
                    Console.WriteLine($"Entity with ID {id} not found.");
                    return;
                }

                if (IsEntityExpired(entity))
                {
                    HandleEntityExpired(entity);
                }
                else
                {
                    await UpdateEntityAsync(hubContext, entity);
                }
            }
        }

        protected abstract Task<object?> GetEntityAsync(AppDbContext appDbContext, int id);
        protected abstract bool IsEntityExpired(object entity);
        protected abstract void HandleEntityExpired(object entity);
        protected abstract Task UpdateEntityAsync(IHubContext<Hubs.GameHub> hubContext, object entity);
    }
}
