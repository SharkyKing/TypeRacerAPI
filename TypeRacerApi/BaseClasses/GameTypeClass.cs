using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameTypeClass
    {
        public int Id { get; set; }
        public string GameTypeName { get; set; }
        [JsonIgnore]
        public virtual ICollection<GameClass> Games { get; set; } 
    }
}
