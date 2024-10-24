using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameBase
    {
        public int Id { get; set; }
        public int GameLevelId { get; set; } 
        public int GameTypeId { get; set; }  
        public string Words { get; set; }
        public bool IsOpen { get; set; } = true;
        public bool IsOver { get; set; } = false;
        public long StartTime { get; set; } = 0;
        public virtual ICollection<PlayerBase> Players { get; set; }
        [JsonIgnore]
        public virtual GameLevelBase GameLevel { get; set; }
        [JsonIgnore]
        public virtual GameTypeBase GameType { get; set; }
    }
}
