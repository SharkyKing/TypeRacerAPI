using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.FactoryMethod.Player
{
    public class PlayerFactories
    {
        public class PlayerGuestFactory : IPlayerFactory
        {
            public PlayerBase CreatePlayer(int gameId, string nickName, string socketId)
            {
                return new PlayerGuest(gameId, nickName, socketId);
            }
        }

        public class PlayerLeaderFactory : IPlayerFactory
        {
            public PlayerBase CreatePlayer(int gameId, string nickName, string socketId)
            {
                return new PlayerLeader(gameId, nickName, socketId);
            }
        }
    }
}
