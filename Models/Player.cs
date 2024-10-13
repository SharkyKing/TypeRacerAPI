using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TypeRacerAPI.Models;

public class Player
{
    public int Id { get; set; }
    public int CurrentWordIndex { get; set; } = 0;
    public string SocketID { get; set; }
    public bool IsPartyLeader { get; set; } = false;
    public int WPM { get; set; } = -1;
    public string NickName { get; set; }
    public int GameId { get; set; }

    [JsonIgnore]
    public virtual Game Game { get; set; }
}
