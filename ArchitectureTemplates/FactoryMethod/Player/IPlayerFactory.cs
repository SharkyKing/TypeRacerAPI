using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.ArchitectureTemplates.FactoryMethod.Player
{
    public interface IPlayerFactory
    {
        PlayerBase CreatePlayer(int gameId, string nickName, string socketId);
    }
}
