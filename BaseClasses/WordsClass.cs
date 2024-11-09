using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class WordsClass
    {
        public int Id { get; set; }
        public int GameLevelId { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public virtual GameLevelClass GameLevel { get; set; }
    }
}
