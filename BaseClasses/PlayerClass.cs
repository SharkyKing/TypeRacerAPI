using System.Text.Json.Serialization;
using TypeRacerAPI.DesignPatterns.Factory.Player.Interface;

namespace TypeRacerAPI.BaseClasses
{
    public class PlayerClass : IPlayer
    {
        public int? Id { get; set; } = null;
        public int CurrentWordIndex { get; set; } = 0;
        public int MistakeCount { get; set; } = 0;
        public string ConnectionGUID { get; set; }
        public bool IsPartyLeader { get; set; } = false;
        public bool IsSpectator { get; set; } = false;
        public bool IsInitialized { get; set; } = false;
        public bool Finished { get; set; } = false;
        public bool IsConnected { get; set; } = true;
        public bool InputEnabled { get; set; } = true;
        public bool WordVisible { get; set; } = true;
        public int WPM { get; set; } = -1;
        public string NickName { get; set; }
        public int? WordsStyleId { get; set; } = 1;
        [JsonIgnore]
        public WordsStyleClass WordsStyle { get; set; } 
        [JsonIgnore]
        public int? GameId { get; set; }
        [JsonIgnore]
        public GameClass Game { get; set; }
        [JsonIgnore]
        public ICollection<PlayerPowerUseClass> PlayerPowerUses { get; set; }
        public ICollection<GameLogClass> GameLog { get; set; }
        public PlayerClass()
        {
            SetupPlayer();
        }

        public virtual void SetupPlayer()
        {
            IsInitialized = true;
        }
    }
}
