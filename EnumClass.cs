namespace TypeRacerAPI
{
    public static class EnumClass
    {
        public enum HubCall
        {
            Done, 
            UpdateGame, 
            ReceiveConnectionId, 
            TimerClient,
            CooldownTimer,
            SendMessageToGame
        }

        public enum PlayerGameEndType
        {
            Won, 
            Lost, 
            Tie
        }
    }
}
