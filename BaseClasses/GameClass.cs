using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TypeRacerAPI.DesignPatterns.State;

namespace TypeRacerAPI.BaseClasses
{
    public class GameClass
    {
        public int Id { get; set; }
        public int GameLevelId { get; set; } 
        public int GameTypeId { get; set; }  
        public string Words { get; set; }
        public bool IsOpen { get; set; } = true;
        public bool IsOver { get; set; } = false;
        public long StartTime { get; set; } = 0;
        public virtual ICollection<PlayerClass> Players { get; set; }
        public virtual ICollection<GameLogClass> GameLog { get; set; }
        [JsonIgnore]
        public virtual GameLevelClass GameLevel { get; set; }
        [JsonIgnore]
        public virtual GameTypeClass GameType { get; set; }

        [NotMapped]
        public IGameState State { get; set; } = new ActiveState();

		public void SetState(IGameState state)
		{
			State = state;
		}

		public void HandleState(IServiceProvider serviceProvider)
		{
			State.Handle(this, serviceProvider);
		}
	}
}
