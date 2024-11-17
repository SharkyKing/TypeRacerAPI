
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.TimeAttack.ConcreteProducts;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Interface;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.TimeAttack
{
    public class TimeAttackGame : IGameFactory
    {
        public AppDbContext appDbContext { get; set; }
        public TimeAttackGame(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }
        public GameClass CreateAdvancedGame()
        {
            return new TimeAttackAdvanced(appDbContext);
        }

        public GameClass CreateBeginnerGame()
        {
            return new TimeAttackBeginner(appDbContext);
        }

        public GameClass CreateNormalGame()
        {
            return new TimeAttackNormal(appDbContext);
        }
    }
}
