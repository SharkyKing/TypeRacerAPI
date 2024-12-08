namespace TypeRacerAPI.DesignPatterns.Mediator
{
    public interface IMediator
    {
        Task NotifyAsync(object sender, string ev);
    }
}
