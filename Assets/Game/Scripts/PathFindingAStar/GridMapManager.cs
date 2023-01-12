using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMapManager : MonoSingleton<GridMapManager>
{
    public Dictionary<Vector2Int, OverlayTile> map;
    public GameObject overlayPrefab { get => _overlayPrefab; }
    public GameObject overlayContainer { get => _overlayContainer; set => _overlayContainer = value; }
    public Tilemap usableTileMap { get => _usableTileMap; set => _usableTileMap = value; }

    [SerializeField] private GameObject _overlayPrefab;
    [SerializeField] private GameObject _overlayContainer;
    [SerializeField] private Tilemap _usableTileMap;

    void Start()
    {
        CreateTiles();
    }

    private void CreateTiles()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();

        BoundsInt bounds = _usableTileMap.cellBounds;
        //Looping through the map tiles      
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                if (_usableTileMap.HasTile(new Vector3Int(x, y, 1)) && !map.ContainsKey(new Vector2Int(x, y)))
                {
                    var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform).GetComponent<OverlayTile>();
                    var cellWorldPosition = _usableTileMap.GetCellCenterWorld(new Vector3Int(x, y, 1));
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = _usableTileMap.GetComponent<TilemapRenderer>().sortingOrder;
                    overlayTile.gridLocation = new Vector3Int(x, y, 1);

                    map.Add(new Vector2Int(x, y), overlayTile);
                }
            }
        }
    }


    public List<OverlayTile> GetAllTiles()
    {
        return map.Values.ToList();
    }

    public OverlayTile GetStandingOnTile(Vector2Int TileToCheck)
    {

        if (map.ContainsKey(TileToCheck))
        {
            return map[TileToCheck];
        }
        else
        {
            return null;
        }
    }

    public OverlayTile GetNearestOnTile(Vector2 position)
    {
        Vector2Int nearestTilePosition = map.Keys.OrderBy(p => Vector2.Distance(p, position)).First();
        if (map.ContainsKey(nearestTilePosition))
        {
            return map[nearestTilePosition];
        }
        else
        {
            return null;
        }
    }

    public void ShowTileMaps()
    {
        foreach (var tile in map.Values)
        {
            tile.ShowTile();
        }
    }

    public void HideTileMaps()
    {
        foreach (var tile in map.Values)
        {
            tile.HideTile();
        }
    }
}

