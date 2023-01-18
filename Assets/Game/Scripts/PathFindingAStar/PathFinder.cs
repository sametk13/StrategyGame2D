using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    //A* Pathfinding Implementation

    private Dictionary<Vector2Int, OverlayTile> searchableTiles;


    public List<OverlayTile> FindPath(OverlayTile startTile, OverlayTile endTile, ref OverlayTile previousEndTile)
    {
        // Initialize the searchable tiles dictionary
        searchableTiles = GridMapManager.Instance.map;


        // Initialize the open and closed lists
        List<OverlayTile> openList = new List<OverlayTile>();
        HashSet<OverlayTile> closedList = new HashSet<OverlayTile>();


        OverlayTile start = startTile;
        OverlayTile end = endTile;


        if (end.isBlocked)
        {
            end = GetUsableClosestTile(end);
        }


        // Add the starting tile to the open list
        openList.Add(start);

        // While there are still tiles in the open list
        while (openList.Count > 0)
        {
            // Get the tile with the lowest F value (F = G + H)
            OverlayTile currentTile = openList.OrderBy(t => t.F).First();

            // Remove the current tile from the open list and add it to the closed list
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            // If the current tile is the end tile, return the path
            if (currentTile == end)
            {
                end.isBlocked = true;
                previousEndTile = end;
                return GetFinishedList(start, end);
            }

            // Get the neighboring tiles
            List<OverlayTile> neighbors = GetNeightbourOverlayTiles(currentTile);

            // Iterate through each neighboring tile
            foreach (var neighbor in neighbors)
            {
                // If the tile is blocked or already in the closed list, skip it
                if (neighbor.isBlocked || closedList.Contains(neighbor))
                {
                    continue;
                }

                // Calculate the G (movement cost) and H (heuristic) values for the tile
                int movementCost = currentTile.G + GetManhattenDistance(currentTile, neighbor);
                int heuristic = GetManhattenDistance(neighbor, end);

                // If the tile is not in the open list, or the new G value is lower than the previous one, update the tile's values
                if (!openList.Contains(neighbor) || movementCost < neighbor.G)
                {
                    neighbor.G = movementCost;
                    neighbor.H = heuristic;
                    neighbor.previous = currentTile;

                    // If the tile is not in the open list, add it
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // If no path is found, return an empty list
        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile tile)
    {
        //Calculating Manhattan
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    public OverlayTile GetUsableClosestTile(OverlayTile currentOverlayTile)
    {
        var neighbours = GetNeightbourOverlayTiles(currentOverlayTile);
        var queue = new Queue<OverlayTile>(neighbours);

        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();
            if (!currentTile.isBlocked)
                return currentTile;
            var currentTileNeighbours = GetNeightbourOverlayTiles(currentTile);
            for (int i = 0; i < currentTileNeighbours.Count; i++)
            {
                queue.Enqueue(currentTileNeighbours[i]);
            }
        }
        return null;
    }

    public List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
    {

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //right
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //right down
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y - 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //right up
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y + 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //left down
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y - 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //left up
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y + 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //top
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        return neighbours;
    }

}
