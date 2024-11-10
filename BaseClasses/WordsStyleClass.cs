using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class WordsStyleClass
    {
        public int Id { get; set; }
        public string StyleName { get; set; }
        public string? fontStyle { get; set; }
        public string? fontWeight { get; set; }
        public string? fontFamily { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerClass> Players { get; set; }
    }
}
