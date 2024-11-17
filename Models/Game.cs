namespace TypeRacerAPI.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Words { get; set; }
        public bool IsOpen { get; set; } = true;
        public bool IsOver { get; set; } = false;
        public long StartTime { get; set; } = 0;
        public virtual ICollection<Player> Players { get; set; }
    }

}
