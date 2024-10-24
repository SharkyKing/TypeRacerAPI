using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.AbstractFactory.Game
{
    public interface IGameFactory
    {
        GameBase CreateBeginnerGame();
        GameBase CreateAdvancedGame();
        GameBase CreateNormalGame();
    }
}
