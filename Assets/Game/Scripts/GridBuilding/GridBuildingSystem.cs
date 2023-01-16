using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridBuildingSystem : MonoSingleton<GridBuildingSystem>
{
    public static Action<Building> OnBuild;
    public static Action<Building> OnCancel;

    public UnityEvent Builded;
    public UnityEvent Canceled;

    private Building _building;
    private OverlayTile _prevTile;
    private List<OverlayTile> _prevTileArea = new List<OverlayTile>();

    private void Update()
    {
        if (_building != null && !_building.isPlaced)
        {
            RaycastHit2D? hit = Raycaster.GetMouseRaycastHit();
            if (hit == null) return;

            OverlayTile currentTile = hit.Value.collider.GetComponent<OverlayTile>();
            if (currentTile == null) return;

            if (currentTile && _prevTile != currentTile)
            {
                _prevTile = currentTile;
                FollowBuilding(currentTile);
            }

            if (Mouse.current.leftButton.wasPressedThisFrame && CanTakeArea(_building.area, GetTilesBlock(_building.area, currentTile).ToArray()))
            {
                _building.build();

                var placedTiles = GetTilesBlock(_building.area, currentTile);
                for (int i = 0; i < placedTiles.Count; i++)
                {
                    placedTiles[i].isBlocked = true;
                }
                ClearArea();
                OnBuild?.Invoke(_building);
                Builded?.Invoke();
                _building = null;
            }

            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                OnCancel?.Invoke(_building);
                Canceled?.Invoke();

                ClearArea();
                Destroy(_building.gameObject);
            }
        }
    }

    public void InitializeBuilding(Building building) 
    {
        _building = building;
    }
    private void FollowBuilding(OverlayTile _targetTile)
    {
        ClearArea();

        _building.transform.localPosition = _targetTile.transform.position;

        _building.area.position = _targetTile.gridLocation;

        BoundsInt buildingArea = _building.area;

        OverlayTile[] tileArray = GetTilesBlock(buildingArea, _targetTile).ToArray();

        ChangeColorOnTileConvenience(buildingArea, tileArray);
        _prevTileArea = new List<OverlayTile>(tileArray.ToList());
    }

    private void ChangeColorOnTileConvenience(BoundsInt area, OverlayTile[] tileArray)
    {
        Color newColor = Color.green;

        if (!CanTakeArea(area,tileArray))       
            newColor = Color.red;
        
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i].ShowTile();
            tileArray[i].SetTileColor(newColor);
        }
    }

    private List<OverlayTile> GetTilesBlock(BoundsInt _area, OverlayTile _targetTile)
    {
        List<OverlayTile> tiles = new List<OverlayTile>();

        for (int x = 0; x < _area.size.x; x++)
        {
            for (int y = 0; y < _area.size.y; y++)
            {
                Vector2Int locationToCheck = new Vector2Int(_targetTile.gridLocation.x + x, _targetTile.gridLocation.y + y);

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
