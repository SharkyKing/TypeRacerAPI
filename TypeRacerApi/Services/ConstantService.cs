using static TypeRacerAPI.EnumClass;

namespace TypeRacerAPI.Services
{
    public static class ConstantService
    {
        public static int StartGameCountdownSeconds = 1;
        public static int GameCountdownSeconds = 60;
        public static int RewindWordCount = 3;
        public static int TimedPowerSeconds = 5;
        public static bool IsDevelopment = true;

        #region TEST CONSTANTS
        public static int GameCountdownSecondsTest = 60;
        public static string TestWords = "test test test";
        #endregion

        public static Dictionary<HubCall, string> HubCalls = new Dictionary<HubCall, string>()
        {
            { HubCall.Done, "done" },
            { HubCall.ReceiveConnectionId, "ReceiveConnectionId" },
            { HubCall.UpdateGame, "UpdateGame" },
            { HubCall.TimerClient, "timerClient" },
            { HubCall.CooldownTimer, "cooldowntimer" },
            { HubCall.SendMessageToGame, "SendMessageToGame" }
        };
    }
}
