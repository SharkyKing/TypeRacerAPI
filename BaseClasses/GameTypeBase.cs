namespace TypeRacerAPI.BaseClasses
{
    public class GameTypeBase
    {
        public int Id { get; set; }
        public string GameTypeName { get; set; }
        public virtual ICollection<GameBase> Games { get; set; } 
    }
}
