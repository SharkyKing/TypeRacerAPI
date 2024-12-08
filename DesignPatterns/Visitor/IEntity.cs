namespace TypeRacerAPI.DesignPatterns.Visitor
{
    public interface IEntity
    {
        void Accept(IEntityVisitor visitor);
    }
}
