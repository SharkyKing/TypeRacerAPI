using System.Net.Sockets;
using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.FactoryMethod.Player
{
    public class PlayerGuest : PlayerBase
    {
        public PlayerGuest(int gameId, string nickName, string socketId)
        {
            GameId = gameId;
            NickName = nickName;
            SocketID = socketId;
            IsPartyLeader = false; 
        }
    }

    public class PlayerLeader : PlayerBase
    {
        public PlayerLeader(int gameId, string nickName, string socketId)
        {
            GameId = gameId;
            NickName = nickName;
            SocketID = socketId;
            IsPartyLeader = true;
        }
    }
}
