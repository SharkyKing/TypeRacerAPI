using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Singleton.LevelTexts;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType.ConcreteProducts
{
    public class FluentTypeNormal : GameClass
    {
        public FluentTypeNormal(AppDbContext appDbContext)
        {
            this.Words = LevelTexts.GetInstance().GetText(Enum.GameLevel.Normal, appDbContext);
        }
    }
}
