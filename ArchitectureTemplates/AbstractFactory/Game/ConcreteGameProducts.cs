using System.Security.AccessControl;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Enums;
using TypeRacerAPI.Services;
using static TypeRacerAPI.Services.LevelTexts;

namespace TypeRacerAPI.ArchitectureTemplates.AbstractFactory.Game
{
    public class BeginnerGame : GameBase
    {
        public readonly LevelText level = LevelText.Beginner;
        public BeginnerGame()
        {
            GameLevelId = 1;
            GameTypeId = 1;
            Words = LevelTexts.GetText(level);
            Players = new List<PlayerBase>();
        }
    }

    public class NormalGame : GameBase
    {
        public readonly LevelText level = LevelText.Normal;
        public NormalGame()
        {
            GameLevelId = 1;
            GameTypeId = 1;
            Words = LevelTexts.GetText(level);
            Players = new List<PlayerBase>();
        }
    }

    public class AdvancedGame : GameBase
    {
        public readonly LevelText level = LevelText.Advanced;
        public AdvancedGame()
        {
            GameLevelId = 2;
            GameTypeId = 1;
            Words = LevelTexts.GetText(level);
            Players = new List<PlayerBase>();
        }
    }
}
