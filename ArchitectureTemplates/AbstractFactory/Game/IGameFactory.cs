using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.ArchitectureTemplates.AbstractFactory.Game
{
    public interface IGameFactory
    {
        GameBase CreateBeginnerGame();
        GameBase CreateAdvancedGame();
        GameBase CreateNormalGame();
    }
}
