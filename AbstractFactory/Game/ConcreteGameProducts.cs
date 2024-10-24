using System.Security.AccessControl;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Enums;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.AbstractFactory.Game
{
    public class BeginnerGame : GameBase
    {
        public BeginnerGame()
        {
            GameLevelId = 1;
            GameTypeId = 1; 
            Words = LevelTexts.GetText("Begginner");
            Players = new List<PlayerBase>();
        }
    }

    public class NormalGame : GameBase
    {
        public NormalGame()
        {
            GameLevelId = 1;
            GameTypeId = 1;
            Words = LevelTexts.GetText("Normal");
            Players = new List<PlayerBase>();
        }
    }

    public class AdvancedGame : GameBase
    {
        public AdvancedGame()
        {
            GameLevelId = 2;
            GameTypeId = 1;
            Words = LevelTexts.GetText("Advanced");
            Players = new List<PlayerBase>();
        }
    }
}
