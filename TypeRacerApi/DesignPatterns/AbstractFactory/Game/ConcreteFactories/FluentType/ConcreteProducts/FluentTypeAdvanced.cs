using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Singleton.LevelTexts;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType.ConcreteProducts
{
    public class FluentTypeAdvanced : GameClass
    {
        public FluentTypeAdvanced(AppDbContext appDbContext)
        {
            this.Words = LevelTexts.GetInstance().GetText(Enum.GameLevel.Advanced, appDbContext);
        }
    }
}
