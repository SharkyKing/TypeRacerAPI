using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class GameLogClass
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? GameId { get; set; }
        public DateTime DateCreated { get; set; }
        public int LogTypeId { get; set; }
        public int? PlayerId { get; set; }
        [JsonIgnore]
        public GameClass Game { get; set; }
        [JsonIgnore]
        public PlayerClass Player{ get; set; }
        [JsonIgnore]
        public LogTypeClass LogType { get; set; }
    }
}
