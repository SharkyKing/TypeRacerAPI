using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerPowerUseClass
    {
        public int Id { get; set; }  
        public int? PlayerId { get; set; }   
        public int PlayerPowerId { get; set; } 
        public bool IsUsed { get; set; }
        public bool IsOnCooldown { get; set; }
        public bool IsReceived { get; set; }
        [JsonIgnore]
        public PlayerClass Player { get; set; }
        [JsonIgnore]
        public PlayerPowerClass PlayerPower { get; set; }
    }
}
