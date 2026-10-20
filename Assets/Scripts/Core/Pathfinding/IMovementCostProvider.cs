public interface IMovementCostProvider
{
    float MinimumCost { get; }
    bool CanEnter(Hex from, Hex to);
    // Must return float >= MinimumCost
    float GetCost(Hex from, Hex to);
}
