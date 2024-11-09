using System.Net.Sockets;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Factory.Player.Interface;

namespace TypeRacerAPI.DesignPatterns.Factory.Player.Products
{
    public class GuestPlayer : PlayerClass, IPlayer
    {
        public GuestPlayer() : base()
        {
            SetupPlayer();
        }

        public void SetupPlayer()
        {
            this.IsPartyLeader = false;
        }
    }
}
