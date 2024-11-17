using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Singleton.LevelTexts;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType.ConcreteProducts
{
    public class FluentTypeBeginner : GameClass
    {
        public FluentTypeBeginner(AppDbContext appDbContext)
        {
            this.Words = LevelTexts.GetInstance().GetText(Enum.GameLevel.Beginner, appDbContext);
        }
    }
}
