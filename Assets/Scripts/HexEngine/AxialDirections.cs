public static class AxialDirections
{
    public static readonly AxialCoordinate[] Directions =
    {
        new AxialCoordinate(+1, 0),  // E
        new AxialCoordinate(0, +1),  // SE
        new AxialCoordinate(-1, +1), // SW
        new AxialCoordinate(-1, 0),  // W
        new AxialCoordinate(0, -1),  // NW
        new AxialCoordinate(+1, -1)  // NE
    };

    public static AxialCoordinate FromDirection(AxialCardinalDirections dir)
        => Directions[(int)dir];
}

public enum AxialCardinalDirections
{
    E = 0,
    SE = 1,
    SW = 2,
    W = 3,
    NW = 4,
    NE = 5
}