namespace Abstractions
{
    public interface IUnitProductionTask : IIconHolder
    {
    	string UnitName { get; }
    	float TimeLeft { get; }
    	float ProductionTime { get; }
    }
}