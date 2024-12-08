namespace TypeRacerAPI.DesignPatterns.Interpretator
{
    public abstract class PowerCastExpression
    {
        public abstract bool Interpret(int? playerId, out int victimPlayerId, string powerCast);
    }

}
