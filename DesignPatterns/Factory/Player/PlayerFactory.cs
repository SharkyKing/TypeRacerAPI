using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Factory.Player.Enum;
using TypeRacerAPI.DesignPatterns.Factory.Player.Products;

namespace TypeRacerAPI.DesignPatterns.Factory.Player
{
    public class PlayerFactory
    {
        int gameId;
        string nickName;
        string ConnectionGUID;

        public PlayerFactory(int gameId, string nickName, string connectionGUID)
        {
            this.gameId = gameId;
            this.nickName = nickName;
            this.ConnectionGUID = connectionGUID;
        }

        public PlayerClass CreatePlayer(PlayerType playerType)
        {
            PlayerClass player;

            switch (playerType)
            {
                case PlayerType.Leader:
                    player = new LeaderPlayer();
                    break;
                case PlayerType.Guest:
                    player = new GuestPlayer();
                    break;
                case PlayerType.Spectator:
                    player = new SpectatorPlayer();
                    break;
                default:
                    player = null; 
                    break;
            }

            if (player != null)
            {
                player.GameId = this.gameId;
                player.NickName = this.nickName;
                player.ConnectionGUID = this.ConnectionGUID;
            }

            return player;
        }
    }
}
