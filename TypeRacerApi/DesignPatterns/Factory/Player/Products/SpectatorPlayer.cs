using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Factory.Player.Interface;

namespace TypeRacerAPI.DesignPatterns.Factory.Player.Products
{
    public class SpectatorPlayer : PlayerClass
    {
        public SpectatorPlayer() : base()
        {
            SetupPlayer();
        }

        public void SetupPlayer()
        {
            this.IsSpectator = true;
        }
    }
}
