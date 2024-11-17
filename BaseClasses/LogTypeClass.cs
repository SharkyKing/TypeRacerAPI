using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class LogTypeClass
    {
        public int Id { get; set; }
        public string LogTypeName { get; set; }

        [JsonIgnore]
        public ICollection<GameLogClass> GameLog { get; set; }

    }
}
