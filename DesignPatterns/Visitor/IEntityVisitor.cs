using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Strategy.PowerStrategies;

namespace TypeRacerAPI.DesignPatterns.Visitor
{
    public interface IEntityVisitor
    {
        ValueTask Visit(FreezePower game);
        ValueTask Visit(InvisiblePower player);
        ValueTask Visit(RewindPower power);
    }

}
