using System.Collections.Generic;

public class Pathfinder
{
    private readonly HexGrid _hexGrid;

    public Pathfinder(HexGrid hexGrid)
    {
        _hexGrid = hexGrid;
    } 

    public AStarPath AStar(AxialCoordinate start, AxialCoordinate goal, IMovementCostProvider costProvider)
    {
        if (!_hexGrid.TryGetHex(start, out _)) return null;

        if (!_hexGrid.TryGetHex(goal, out _)) return null;
        
        var nodes = new Dictionary<AxialCoordinate, PathNode>();

        PathNode startNode = GetNode(start, nodes);
        PathNode goalNode = GetNode(goal, nodes);
        List<PathNode> openList = new List<PathNode>();
        openList.Add(startNode);
        List<PathNode> closedList = new List<PathNode>();

        startNode.G = 0f;
        startNode.H = Heuristic(startNode, goalNode, costProvider);
        startNode.F = startNode.G + startNode.H;

        while (openList.Count > 0)
        {
            PathNode currentNode = FindLowestF(openList);
            if (currentNode == goalNode)
            {
                return ReconstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighbors = GetNeighbors(currentNode, nodes);
            foreach (PathNode neighbor in neighbors)
            {
                if (closedList.Contains(neighbor)) continue;

                if (!Passable(currentNode.Coord, neighbor.Coord, costProvider)) continue;

                float stepCost = costProvider.GetCost(_hexGrid.GetHex(currentNode.Coord),
                    _hexGrid.GetHex(neighbor.Coord));

                float tentativeG = currentNode.G + stepCost;

                if (tentativeG >= neighbor.G) continue;

                neighbor.Parent = currentNode;
                neighbor.StepCostFromParent = stepCost;
                neighbor.G = tentativeG;
                neighbor.H = Heuristic(neighbor, goalNode, costProvider);
                neighbor.F = neighbor.G + neighbor.H;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    PathNode GetNode(AxialCoordinate c, Dictionary<AxialCoordinate, PathNode> nodes)
    {
        if (!nodes.TryGetValue(c, out var n))
        {
            n = new PathNode(c);
            nodes.Add(c, n);
        }
        return n;
    }

    private float Heuristic(PathNode start, PathNode goal, IMovementCostProvider costProvider)
    {
        return costProvider.MinimumCost * AxialGeometry.DistanceBetweenCoords(start.Coord, goal.Coord);
    }

    private bool Passable(AxialCoordinate currentCoord, AxialCoordinate neighborCoord, IMovementCostProvider costProvider)
    {
        if (!_hexGrid.TryGetHex(neighborCoord, out var neighborHex)) return false;
        
        return costProvider.CanEnter(_hexGrid.GetHex(currentCoord), neighborHex);
    }

    private PathNode FindLowestF(List<PathNode> list)
    {
        PathNode lowestF = list[0];
        foreach (PathNode node in list)
        {
            if (node.F < lowestF.F) lowestF = node;
        }
        return lowestF;
    }

    private AStarPath ReconstructPath(PathNode goalNode)
    {
        List<AStarPathStep> steps = new();
        PathNode current = goalNode;

        while (current.Parent != null)
        {
            steps.Add(new AStarPathStep(current.Parent.Coord, current.Coord, current.StepCostFromParent));

            current = current.Parent;
        }

        steps.Reverse();

        return new AStarPath(steps, goalNode.G);
    }

    private List<PathNode> GetNeighbors(PathNode node, Dictionary<AxialCoordinate, PathNode> nodes)
    {
        List<PathNode> neighbors = new List<PathNode>();
        foreach (AxialCoordinate direction in AxialDirections.Directions)
        {
            AxialCoordinate neighborCoord = node.Coord + direction;

            if (_hexGrid.TryGetHex(neighborCoord, out _))
            {
                neighbors.Add(GetNode(neighborCoord, nodes));
            }
        }
        return neighbors;
    }
}

public class PathNode 
{ 
    public AxialCoordinate Coord; 
    public float G; 
    public float H;
    public float F; 
    public float StepCostFromParent;
    public PathNode Parent; 
    public PathNode(AxialCoordinate coord)
    {
        Coord = coord;
        G = float.PositiveInfinity;
    }
}

public sealed class AStarPath
{
    public IReadOnlyList<AStarPathStep> Steps { get; }
    public float TotalCost { get; }

    public AStarPath(List<AStarPathStep> steps, float totalCost)
    {
        Steps = steps;
        TotalCost = totalCost;
    }
}

public readonly struct AStarPathStep
{
    public AxialCoordinate From { get; }
    public AxialCoordinate To { get; }
    public float Cost { get; }

    public AStarPathStep(
        AxialCoordinate from,
        AxialCoordinate to,
        float cost)
    {
        From = from;
        To = to;
        Cost = cost;
    }
}