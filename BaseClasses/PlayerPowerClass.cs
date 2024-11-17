using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerPowerClass
    {
        public int Id { get; set; }
        public string PlayerPowerName { get; set; }
        public string ImagePath { get; set; }
        public string PlayerPowerKey { get; set; }
        public int CooldownTime { get; set; }
        public bool IsTimedPower { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerPowerUseClass> PlayerPowerUses { get; set; }
    }
}
