using System;
using TypeRacerAPI.DesignPatterns.Mediator;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.TemplateMethod;

namespace TypeRacerAPI.DesignPatterns.Observer
{
    public class PlayerObserver : IObserver
    {
        public int playerId;

        public bool isExpired { get; set; }

		private readonly IMediator _mediator;

		public PlayerObserver(IMediator mediator)
		{
			_mediator = mediator;
		}

		public void SetPlayerId(int id)
        {
            playerId = id;
        }

        public async ValueTask Update(IServiceScopeFactory serviceProvider)
		{
			try
			{

				await _mediator.NotifyAsync(this, "Update");
				//var playerUpdate = new PlayerObserverUpdate();
                //await playerUpdate.UpdateAsync(serviceProvider, playerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating player with ID {playerId}: {ex.Message}");
            }
        }
    }
}
