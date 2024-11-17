using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Interface
{
    public interface IGameFactory
    {
        GameClass CreateBeginnerGame();
        GameClass CreateAdvancedGame();
        GameClass CreateNormalGame();
    }
}
