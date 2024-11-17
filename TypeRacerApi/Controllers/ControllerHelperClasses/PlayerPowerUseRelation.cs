using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.Controllers.ControllerHelperClasses
{
    public class PlayerPowerUseRelation
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string PlayerPowerKey { get; set; }
        public bool IsUsed { get; set; }
    }
}
