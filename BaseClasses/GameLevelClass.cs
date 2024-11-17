using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameLevelClass
    {
        public int Id { get; set; }
        public string GameLevelName { get; set; }
        [JsonIgnore]
        public virtual ICollection<GameClass> Games { get; set; }
        [JsonIgnore]
        public virtual ICollection<WordsClass> Words { get; set; }
    }
}
