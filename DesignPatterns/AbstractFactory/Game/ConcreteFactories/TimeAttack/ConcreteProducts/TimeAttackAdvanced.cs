using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Singleton.LevelTexts;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.TimeAttack.ConcreteProducts
{
    public class TimeAttackAdvanced : GameClass
    {
        public TimeAttackAdvanced(AppDbContext appDbContext)
        {
            this.Words = LevelTexts.GetInstance().GetText(Enum.GameLevel.Advanced, appDbContext);
        }
    }
}
