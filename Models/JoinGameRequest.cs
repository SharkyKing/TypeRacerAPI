namespace MyAspNetCoreApi.Models
{
    public class JoinGameRequest
    {
        public int GameId { get; set; }
        public string NickName { get; set; }
        public string SocketID { get; set; } 
    }

}
