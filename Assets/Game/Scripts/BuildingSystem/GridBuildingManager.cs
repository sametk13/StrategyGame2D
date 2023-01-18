using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridBuildingManager : MonoSingleton<GridBuildingManager>
{
    // static events that can be subscribed to by other classes
    public static Action<Building> OnBuild;
    public static Action<Building> OnCancel;

    // UnityEvents that can be subscribed to in the Unity editor
    public UnityEvent Builded;
    public UnityEvent Canceled;

    private Building _building;
    private OverlayTile _prevTile;
    private List<OverlayTile> _prevTileArea = new List<OverlayTile>();

    private void Update()
    {
        // Check if a building is currently being placed
        if (_building != null && !_building.isPlaced)
        {
            // Get the tile that the mouse is currently over
            OverlayTile currentTile = GetTileToMousePos();
            if (currentTile == null) return;

            // Check if the mouse has moved to a new tile
            if (currentTile && _prevTile != currentTile)
            {
                _prevTile = currentTile;
                FollowBuilding(currentTile);
            }

            // Check if the left mouse button was pressed this frame
            if (Mouse.current.leftButton.wasPressedThisFrame && CanTakeArea(_building.area, GetTilesBlock(_building.area, currentTile).ToArray()))
            {
                Place(currentTile);
            }

            // Check if the right mouse button was pressed this frame
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CancelPlacing();
            }
        }
    }

    private OverlayTile GetTileToMousePos()
    {
        // Use a raycast to get the tile that the mouse is currently over
        RaycastHit2D? hit = Raycaster.GetMouseRaycastHit();
        if (hit == null) return null;

        OverlayTile currentTile = hit.Value.collider.GetComponent<OverlayTile>();

        return currentTile;
    }

    private void Place(OverlayTile currentTile)
    {
        // Get the tiles that the building will be placed on
        var placedTiles = GetTilesBlock(_building.area, currentTile);
        for (int i = 0; i < placedTiles.Count; i++)
        {
            placedTiles[i].isBlocked = true;
        }
        // Finish placing the building
        _building.build();
        ClearArea();
        OnBuild?.Invoke(_building);
        Builded?.Invoke();
        _building = null;
    }
    private void CancelPlacing()
    {
        OnCancel?.Invoke(_building);
        Canceled?.Invoke();

        ClearArea();
        Destroy(_building.gameObject);
    }

    public void InitializeBuilding(Building building)
    {
        _building = building;
        _building.DissableCollider();
    }
    private void FollowBuilding(OverlayTile targetTile)
    {
        ClearArea();

        Vector3 newPos = targetTile.transform.position;
        newPos.z = -1;


        _building.transform.position = newPos;

        _building.area.position = targetTile.gridLocation;

        BoundsInt buildingArea = _building.area;

        OverlayTile[] tileArray = GetTilesBlock(buildingArea, targetTile).ToArray();

        ChangeColorOnTileConvenience(buildingArea, tileArray);
        _prevTileArea = new List<OverlayTile>(tileArray.ToList());
    }

    private void ChangeColorOnTileConvenience(BoundsInt area, OverlayTile[] tileArray)
    {
        Color newColor = Color.green;

        if (!CanTakeArea(area, tileArray))
            newColor = Color.red;

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].ShowTile();
            tileArray[i].SetTileColor(newColor);
        }
    }

    private List<OverlayTile> GetTilesBlock(BoundsInt area, OverlayTile targetTile)
    {
        List<OverlayTile> tiles = new List<OverlayTile>();

        for (int x = 0; x < area.size.x; x++)
        {
            for (int y = 0; y < area.size.y; y++)
            {
                Vector2Int locationToCheck = new Vector2Int(targetTile.gridLocation.x + x, targetTile.gridLocation.y + y);

                OverlayTile currentTile = GridMapManager.Instance.GetStandingOnTile(locationToCheck);
                if (currentTile)
                {
                    tiles.Add(currentTile);
                }
            }
        }
        return tiles;
    }

    private void ClearArea()
    {
        for (int i = 0; i < _prevTileArea.Count; i++)
        {
            _prevTileArea[i].SetTileColor(Color.white);
            _prevTileArea[i].HideTile();
        }
    }

    public bool CanTakeArea(BoundsInt area, OverlayTile[] tileArray)
    {
        if (area.size.x * area.size.y > tileArray.Count())
            return false;

        foreach (OverlayTile _tile in tileArray)
        {
            if (_tile.isBlocked)
            {
                return false;
            }
        }
        return true;
    }
}
