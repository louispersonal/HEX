using System.Collections.Generic;

public static class AxialDirections
{
    public static readonly AxialCoordinate[] Directions =
    {
        new (+1, 0),  // E
        new (0, +1),  // SE
        new (-1, +1), // SW
        new (-1, 0),  // W
        new (0, -1),  // NW
        new (+1, -1)  // NE
    };

    public static AxialCoordinate FromDirection(AxialCardinalDirections dir)
        => Directions[(int)dir];

    public static readonly Dictionary<AxialCoordinate, AxialCardinalDirections> DirectionMap = new()
    {
        { new AxialCoordinate (+1, 0), AxialCardinalDirections.E },
        { new AxialCoordinate (0, +1), AxialCardinalDirections.SE },
        { new AxialCoordinate (-1, +1), AxialCardinalDirections.SW },
        { new AxialCoordinate (-1, 0), AxialCardinalDirections.W },
        { new AxialCoordinate (0, -1), AxialCardinalDirections.NW },
        { new AxialCoordinate (+1, -1), AxialCardinalDirections.NE },
    };
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