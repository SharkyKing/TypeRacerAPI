using System.Text.RegularExpressions;

namespace TypeRacerAPI.DesignPatterns.Interpretator
{
    public class RewindPowerCast : PowerCastExpression
    {
        public override bool Interpret(int? playerId, out int victimPlayerId, string powerCast)
        {
            victimPlayerId = 0;

            Regex regex = new Regex(@"\/\/r\/(\d+)");
            var match = regex.Match(powerCast);

            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out victimPlayerId))
                {
                    if (victimPlayerId == playerId)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }

}
