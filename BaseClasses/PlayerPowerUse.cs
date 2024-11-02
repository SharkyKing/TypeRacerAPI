using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerPowerUse
    {
        public int Id { get; set; }  
        public int PlayerId { get; set; }   
        public int PlayerPowerId { get; set; } 
        public bool IsUsed { get; set; }
        [JsonIgnore]
        public PlayerBase Player { get; set; }
        [JsonIgnore]
        public PlayerPowerBase PlayerPower { get; set; }
    }
}
