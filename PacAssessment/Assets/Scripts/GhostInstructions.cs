public struct GhostInstruction
{
    public string PositionName;
    public Direction Direction;

    // Constructor
    public GhostInstruction(
        string positionName,
        Direction direction)
    {
        PositionName = positionName;
        Direction = direction;
    }
}