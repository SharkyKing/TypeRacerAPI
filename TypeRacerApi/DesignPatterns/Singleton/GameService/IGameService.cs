using System.Threading.Tasks;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Controllers.ControllerHelperClasses;

namespace TypeRacerAPI.DesignPatterns.Singleton.GameService
{
    public interface IGameService
    {
        IEnumerable<GameTypeClass> GameTypes { get; }
        IEnumerable<GameLevelClass> GameLevels { get; }
        IEnumerable<PlayerPowerClass> Powers { get; }
        IEnumerable<WordsStyleClass> WordStyles { get; }
        IEnumerable<PlayerGameResultTypeClass> PlayerGameResults { get; }
        Task<GameClass?> GetGame(int gameId);
        ValueTask AddGameAsync(GameClass game);
        ValueTask AddPlayerAsync(GameClass game, PlayerClass player);
        Task<List<PlayerPowerUseRelation>> GetPlayerPowers(int playerId);
        Task<PlayerClass?> GetPlayer(int playerId);
        Task<PlayerClass?> GetPlayerByConnectionGUID(string connectionGUID);
        Task RemovePlayer(int? playerId);
    }
} 