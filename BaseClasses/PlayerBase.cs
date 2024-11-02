using System.Text.Json.Serialization;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerBase
    {
        public int Id { get; set; }
        public int CurrentWordIndex { get; set; } = 0;
        public string SocketID { get; set; }
        public bool IsPartyLeader { get; set; } = false;
        public int WPM { get; set; } = -1;
        public string NickName { get; set; }
        public int GameId { get; set; }
        [JsonIgnore]
        public GameBase Game { get; set; }
        [JsonIgnore]
        public ICollection<PlayerPowerUse> PlayerPowerUses { get; set; }
    }
}
