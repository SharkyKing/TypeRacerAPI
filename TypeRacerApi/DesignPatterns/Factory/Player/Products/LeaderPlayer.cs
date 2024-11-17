using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Factory.Player.Interface;

namespace TypeRacerAPI.DesignPatterns.Factory.Player.Products
{
    public class LeaderPlayer : PlayerClass, IPlayer
    {
        public LeaderPlayer() : base()
        {
            SetupPlayer();
        }

        public void SetupPlayer()
        {
            this.IsPartyLeader = true;
        }
    }
}
