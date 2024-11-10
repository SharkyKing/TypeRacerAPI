
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.FluentType;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.ConcreteFactories.TimeAttack;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Interface;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.DesignPatterns.AbstractFactory.Game
{
    public static class GameCreator
    {
        public static async ValueTask<GameClass?> CreateGameObj(int activeGameType, int activeGameLevel, AppDbContext appDbContext)
        {
            GameClass game = null;
            IGameFactory gameFactory;

            GameLevel level = LogicHelper.GetGameLevelEnum(activeGameLevel);
            GameType type = LogicHelper.GetGameTypeEnum(activeGameType);

            switch (type)
            {
                case GameType.TimeAttack:
                    gameFactory = new TimeAttackGame(appDbContext);
                    break;
                case GameType.FluentType:
                    gameFactory = new FluentTypeGame(appDbContext);
                    break;
                default:
                    gameFactory = new TimeAttackGame(appDbContext);
                    break;
            }

            if(gameFactory != null)
            {
                switch (level)
                {
                    case GameLevel.Beginner:
                        game = gameFactory.CreateBeginnerGame();
                        break;
                    case GameLevel.Normal:
                        game = gameFactory.CreateNormalGame();
                        break;
                    case GameLevel.Advanced:
                        game = gameFactory.CreateAdvancedGame();
                        break;
                    default:
                        game = gameFactory.CreateBeginnerGame();
                        break;
                }
            }

            if (ConstantService.IsDevelopment)
            {
                game.Words = ConstantService.TestWords;
            }

            game.GameLevelId = activeGameLevel;
            game.GameTypeId = activeGameType;
            game.Players = new List<PlayerClass>();
            await appDbContext.Games.AddAsync(game);
            await appDbContext.SaveChangesAsync();
            return game;
        }
    }
}
