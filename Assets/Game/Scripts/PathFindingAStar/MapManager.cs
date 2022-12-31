using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoSingleton<MapManager>
{

    public GameObject overlayPrefab { get => _overlayPrefab; set => _overlayPrefab = value; }
    public GameObject overlayContainer { get => _overlayContainer; set => _overlayContainer = value; }
    public bool ignoreBottomTiles { get => _ignoreBottomTiles; set => _ignoreBottomTiles = value; }

    public Dictionary<Vector2Int, OverlayTile> map;


    [SerializeField] private GameObject _overlayPrefab;
    [SerializeField] private GameObject _overlayContainer;
    [SerializeField] private bool _ignoreBottomTiles;

    void Start()
    {
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderBy(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        map = new Dictionary<Vector2Int, OverlayTile>();

        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;

            //Looping through the map tiles

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {

                        if (z == 0 && ignoreBottomTiles)
                            return;

                        if (tm.HasTile(new Vector3Int(x, y, z)))
                        {
                            if (!map.ContainsKey(new Vector2Int(x, y)))
                            {
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder;
                                overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);

                                map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                            }
                        }
                    }
                }
            }
        }
    }

    public List<OverlayTile> GetSurroundingTiles()
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

