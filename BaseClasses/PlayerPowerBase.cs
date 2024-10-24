namespace TypeRacerAPI.BaseClasses
{
    public class PlayerPowerBase
    {
        public int Id { get; set; }
        public string Game { get; set; }
        public virtual ICollection<GameBase> Games { get; set; }
    }
}
