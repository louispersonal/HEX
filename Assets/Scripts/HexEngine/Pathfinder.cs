using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private const float MINIMUM_COST = 1f;

    public HexGrid HexGrid;

    public Pathfinder(HexGrid hexGrid)
    {
        HexGrid = hexGrid;
    } 

    public List<AxialCoordinate> AStar(AxialCoordinate start, AxialCoordinate goal)
    {
        if (!HexGrid.TryGetHex(start, out _)) return null;

        if (!HexGrid.TryGetHex(goal, out _)) return null;
        
        var nodes = new Dictionary<AxialCoordinate, PathNode>();

        PathNode startNode = GetNode(start, nodes);
        PathNode goalNode = GetNode(goal, nodes);
        List<PathNode> openList = new List<PathNode>();
        openList.Add(startNode);
        List<PathNode> closedList = new List<PathNode>();

        startNode.G = 0f;
        startNode.H = Heuristic(startNode, goalNode);
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

                if (!Passable(neighbor.Coord)) continue;

                float tentativeG = currentNode.G + StepCost(currentNode, neighbor);

                if (!openList.Contains(neighbor)) openList.Add(neighbor);
                else if (tentativeG >= neighbor.G) continue;

                neighbor.Parent = currentNode;
                neighbor.G = tentativeG;
                neighbor.H = Heuristic(neighbor, goalNode);
                neighbor.F = neighbor.G + neighbor.H;
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

    public float Heuristic(PathNode start, PathNode goal)
    {
        return MINIMUM_COST * AxialGeometry.DistanceBetweenCoords(start.Coord, goal.Coord);
    }

    public bool Passable(AxialCoordinate coord)
    {
        if (HexGrid.TryGetHex(coord, out var hex)) return !hex.ExtraData.IsSea;
        return false;
    }

    public float StepCost(PathNode start, PathNode neighbor)
    {
        return AxialGeometry.DistanceBetweenCoords(start.Coord, neighbor.Coord);
    }

    public PathNode FindLowestF(List<PathNode> list)
    {
        PathNode lowestF = list[0];
        foreach (PathNode node in list)
        {
            if (node.F < lowestF.F) lowestF = node;
        }
        return lowestF;
    }

    public List<AxialCoordinate> ReconstructPath(PathNode node)
    {
        List<AxialCoordinate> path = new List<AxialCoordinate>();
        PathNode current = node;
        while (current != null)
        {
            path.Insert(0, current.Coord);
            current = current.Parent;
        }
        return path;
    }

    public List<PathNode> GetNeighbors(PathNode node, Dictionary<AxialCoordinate, PathNode> nodes)
    {
        List<PathNode> neighbors = new List<PathNode>();
        foreach (AxialCoordinate direction in AxialDirections.Directions)
        {
            if (HexGrid.TryGetHex(direction, out var hex))
            {
                neighbors.Add(GetNode(node.Coord + direction, nodes));
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
    public PathNode Parent; 
    public PathNode(AxialCoordinate c) { Coord = c; }
}