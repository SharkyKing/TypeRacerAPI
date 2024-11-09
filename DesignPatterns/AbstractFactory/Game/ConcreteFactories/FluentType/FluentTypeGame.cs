
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType.ConcreteProducts;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Interface;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType
{
    public class FluentTypeGame : IGameFactory
    {
        public AppDbContext appDbContext { get; set; }

        public FluentTypeGame(AppDbContext _appDbContext)
        {
            this.appDbContext = _appDbContext;
        }

        public GameClass CreateAdvancedGame()
        {
            return new FluentTypeAdvanced(appDbContext);
        }

        public GameClass CreateBeginnerGame()
        {
            return new FluentTypeBeginner(appDbContext);
        }

        public GameClass CreateNormalGame()
        {
            return new FluentTypeNormal(appDbContext);
        }
    }
}
