using Godot;
using System;
using System.Collections.Generic;

public class Pathfinding
{   
    Grid grid;
    Player player;

    List<PathfindingNode> openList = new List<PathfindingNode>();
    List<PathfindingNode> closedList = new List<PathfindingNode>();

    public void InitializePathfinding(Grid _grid, Player _player)
    {
        grid = _grid;
        player = _player;
    }

    public List<Vector2> StartPathfinding(Vector2 _start, Vector2 _end)
    {
        ClearOpenAndClosedList();
        bool isFindingPath = true;
        List<Vector2> pathToFollow = new List<Vector2>();

        PathfindingNode currentNode = new PathfindingNode(PathfindingNode.TypeOfNode.Start, null, _start, _start, _end); // Create starting node and set it as current.
        PathfindingNode endNode = new PathfindingNode(PathfindingNode.TypeOfNode.End, null, _end, _start, _end); // Creates the end node
        openList.Add(currentNode);  // Add Starting node to openList

        while(isFindingPath)
        {
            if (IsCurrentNodeTheGoal(currentNode, endNode))  // Goal was found, retrace path and break from loop
            {
                pathToFollow = RetracePath(currentNode);
                isFindingPath = false;
            }

            CreateNeighborNodes(currentNode, _start, _end);  // Add neighbor nodes to open list
            closedList.Add(currentNode);  // Add currentNode to closed list so it won't be searched again
            openList.Remove(currentNode); // Remove from openList so it won't be searched again

            if (openList.Count > 0)
                currentNode = FindNextNode(currentNode);  // Find the next openNode with lowest F score, that node will be new currentNode.

            if (openList.Count == 0)  // No path was found, quit pathfinding
            {
                isFindingPath = false;
            }
        }

        return pathToFollow;
    }

    private PathfindingNode FindNextNode(PathfindingNode _currentNode)
    {
        PathfindingNode lowestScoreNode = openList[0];

        foreach (PathfindingNode openNode in openList)
        {
            if (openNode.F < lowestScoreNode.F)
            {
                lowestScoreNode = openNode;
            }
        }
        return lowestScoreNode;
    }

    private void CreateNeighborNodes(PathfindingNode _currentNode, Vector2 _start, Vector2 _end)
    {
        Vector2[] neighorArray = GetNeighborArray(_currentNode.GridPosition);

        for (int i = 0; i < neighorArray.Length; ++i)
        {
            if (IsNodeInGrid(neighorArray[i]))
            {
                if (IsInClosedList(neighorArray[i]))  // Check if node is found in closed list, if it is go to next neighbor node
                {
                    continue;
                }

                if (IsInOpenList(neighorArray[i]))
                {
                    continue;
                }

                if (!IsWalkable(neighorArray[i]))  // Check if node is walkable, if it isn't then go to next neighbor node
                {
                    continue;
                }

                if (IsCurrentlyOccupied(neighorArray[i]))  // Check if node is occupied, if it is then go to next neighbor node
                {
                    if (!IsCurrentlyOccupiedByPlayer(neighorArray[i]))  // If the node is occupied by the player, then still open the node
                    {
                        continue;
                    }
                }

                // If these checks have passed, then create the node and add it to the openList
                openList.Add(new PathfindingNode(PathfindingNode.TypeOfNode.Walkable, _currentNode, neighorArray[i], _start, _end));
            }
            else  // Node not in grid, continue so a out of bounds error doesn't occur
            {
                continue;
            }
        }
    }

    private Vector2[] GetNeighborArray(Vector2 _nodePosition)
    {
        // Gets positions of all nodes in the 8 directions of given node position, returns them in an array
        Vector2[] neighorArray = new Vector2[8];

        neighorArray[0] = _nodePosition - new Vector2(-1, -1);
        neighorArray[1] = _nodePosition - new Vector2(0, -1);
        neighorArray[2] = _nodePosition - new Vector2(1, -1);
        neighorArray[3] = _nodePosition - new Vector2(1, 0);
        neighorArray[4] = _nodePosition - new Vector2(1, 1);
        neighorArray[5] = _nodePosition - new Vector2(0, 1);
        neighorArray[6] = _nodePosition - new Vector2(-1, 1);
        neighorArray[7] = _nodePosition - new Vector2(-1, 0);

        return neighorArray;
    }

    private bool IsNodeInGrid(Vector2 _nodePosition)
    {
        // Checks to see if node is within the bounds of the tileGrid array

        int x = (int)_nodePosition.x;
        int y = (int)_nodePosition.y;

        if ((x >= 0 && x < grid.GridWidth) && (y >= 0 && y < grid.GridHeight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsInClosedList(Vector2 _nodePosition)
    {
        foreach(PathfindingNode node in closedList)
        {
            if (node.GridPosition.x == _nodePosition.x && node.GridPosition.y == _nodePosition.y)  // The node was found in closed list
            {
                return true;
            }
        }
        return false;
    }

    private bool IsInOpenList(Vector2 _nodePosition)
    {
        foreach(PathfindingNode node in openList)
        {
            if (node.GridPosition.x == _nodePosition.x && node.GridPosition.y == _nodePosition.y)  // The node was found in open list
            {
                return true;
            }
        }
        return false;
    }

    private bool IsWalkable(Vector2 _nodePosition)
    {
        // Checks to see if node is a Floor Tile
        int x = (int)_nodePosition.x;
        int y = (int)_nodePosition.y;
        if (grid.TileGrid[x,y].SelectedTypeOfTile == Tile.TypeOfTile.Floor)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool IsCurrentlyOccupied(Vector2 _nodePosition)
    {
        int x = (int)_nodePosition.x;
        int y = (int)_nodePosition.y;
        if (grid.TileGrid[x,y].IsOccupied)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool IsCurrentlyOccupiedByPlayer(Vector2 _nodePosition)
    {
        int x = (int)_nodePosition.x;
        int y = (int)_nodePosition.y;

        if (grid.TileGrid[x,y].Occupant == player)
        {
            return true;
        }

        return false;
    }

    private bool IsCurrentNodeTheGoal(PathfindingNode _node, PathfindingNode _endNode)
    {
        if (_node.GridPosition == _endNode.GridPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<Vector2> RetracePath(PathfindingNode _currentNode)
    {
        // Trace back the path from the currentNode, which is the end node, by using it's Parent (a came from node), until it reaches the start node.
        // Once the path is traced, flip it.

        List<Vector2> pathToFollow = new List<Vector2>();
        List<Vector2> reversedPathToFollow = new List<Vector2>();

        while(_currentNode.SelectedTypeOfNode != PathfindingNode.TypeOfNode.Start)
        {
            pathToFollow.Add(_currentNode.GridPosition);
            _currentNode = _currentNode.Parent;
        }

        pathToFollow.Reverse();

        return pathToFollow;
    }

    private void ClearOpenAndClosedList()
    {
        // Clears list at start of pathfinding
        openList.Clear();
        closedList.Clear();
    }
}

public class PathfindingNode
{
    public PathfindingNode(TypeOfNode _selectedTypeOfNode, PathfindingNode _parent, Vector2 _gridPosition, Vector2 _start, Vector2 _end)
    {
        SelectedTypeOfNode = _selectedTypeOfNode;
        GridPosition = _gridPosition;
        Parent = _parent;
        G = CalculateGScore(_start);
        H = CalculateHScore(_end);
        F = G + H;
    }

    public enum TypeOfNode { Walkable, NotWalkable, Start, End }
    public TypeOfNode SelectedTypeOfNode { get; set; }
    public Vector2 GridPosition { get; set; }
    public int F  { get; set; }   // Total cost of moving, G + H = F
    public int G   { get; set; }  // Distance between position and start node
    public int H   { get; set; }  // Distance between position and end
    public PathfindingNode Parent { get; set; }

    private int CalculateGScore(Vector2 _start)
    {
        // Get the distance from calculating an L shape
        Vector2 distance = GridPosition - _start;
        int d = (int)Mathf.Abs(distance.x) + (int)Mathf.Abs(distance.y);

        return d;
    }
    private int CalculateHScore(Vector2 _end)
    {
        // Get the distance by using pythagorean theorem
        int a = (int)Mathf.Abs(GridPosition.x) - (int)Mathf.Abs(_end.x);
        int b = (int)Mathf.Abs(GridPosition.y) - (int)Mathf.Abs(_end.y);
        int c = (int)Math.Pow(a, 2) + (int)Math.Pow(b, 2);
        c = (int)Math.Abs(c);

        return c;
    }
}
