using System;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.TemplateMethod;

namespace TypeRacerAPI.DesignPatterns.Observer
{
    public class PlayerObserver : IObserver
    {
        public int playerId;

        public bool isExpired { get; set; }

        public void SetPlayerId(int id)
        {
            playerId = id;
        }

        public async ValueTask Update(IServiceProvider serviceProvider)
        {
            try
            {
                var playerUpdate = new PlayerObserverUpdate();
                await playerUpdate.UpdateAsync(serviceProvider, playerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating player with ID {playerId}: {ex.Message}");
            }
        }
    }
}
