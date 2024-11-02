using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerPowerBase
    {
        public int Id { get; set; }
        public string PlayerPowerName { get; set; }
        public string ImagePath { get; set; }
        public string PlayerPowerKey { get; set; }
        public int CooldownTime { get; set; }
        public bool IsOneTimeUse { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerPowerUse> PlayerPowerUses { get; set; }
    }
}
