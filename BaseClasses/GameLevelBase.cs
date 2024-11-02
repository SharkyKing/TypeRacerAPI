using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameLevelBase
    {
        public int Id { get; set; }
        public string GameLevelName { get; set; }
        [JsonIgnore]
        public virtual ICollection<GameBase> Games { get; set; } 
    }
}
