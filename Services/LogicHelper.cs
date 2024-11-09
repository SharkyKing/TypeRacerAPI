using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.AbstractFactory.Game.Enum;

namespace TypeRacerAPI.Services
{
    public static class LogicHelper
    {
        public static int CalculateWPM(DateTime endTime, DateTime startTime, PlayerClass player)
        {
            var totalTimeInMinutes = (endTime - startTime).TotalMinutes;
            var wordsTyped = player.CurrentWordIndex;
            return (int)(wordsTyped / totalTimeInMinutes);
        }

        public static string CalculateTime(int seconds)
        {
            var minutes = seconds / 60;
            var remainingSeconds = seconds % 60;
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        public static GameLevel GetGameLevelEnum(int gameLevelInt)
        {
            switch (gameLevelInt)
            {
                case 1:
                    return GameLevel.Beginner;
                case 2:
                    return GameLevel.Normal;
                case 3:
                    return GameLevel.Advanced;
                default:
                    return GameLevel.Beginner;
            }
        }

        public static GameType GetGameTypeEnum(int gameLevelInt)
        {
            switch (gameLevelInt)
            {
                case 1:
                    return GameType.TimeAttack;
                case 2:
                    return GameType.FluentType;
                default:
                    return GameType.TimeAttack;
            }
        }
    }
}
