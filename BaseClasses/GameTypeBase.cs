using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameTypeBase
    {
        public int Id { get; set; }
        public string GameTypeName { get; set; }
        [JsonIgnore]
        public virtual ICollection<GameBase> Games { get; set; } 
    }
}
