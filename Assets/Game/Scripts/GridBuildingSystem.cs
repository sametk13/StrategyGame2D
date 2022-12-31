using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoSingleton<GridBuildingSystem>
{
    public static Action<Building> OnBuild;
    public static Action<Building> OnCancel;

    public UnityEvent Builded;
    public UnityEvent Canceled;


    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    [SerializeField] InputActionReference mousePositionReference;
    #region Unity Methods

    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));

        TileMapColorAlphaSetter(MainTilemap, 0f);
    }

    private void Update()
    {
        if (!temp)
        {
            return;
        }


        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        if (!temp.Placed)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(mousePositionReference.action.ReadValue<Vector2>());
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (prevPos != cellPos)
            {
                temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos
                    + new Vector3(.5f, .5f, 0f));
                prevPos = cellPos;
                FollowBuilding();
            }
        }

        if (Input.GetMouseButtonDown(0) && temp.CanBePlaced())
        {
            temp.Place();
            OnBuild?.Invoke(temp);
            Builded?.Invoke();
            temp = null;

            TileMapColorAlphaSetter(MainTilemap, 0f);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnCancel?.Invoke(temp);
            Canceled?.Invoke();

            ClearArea();
            Destroy(temp.gameObject);

            TileMapColorAlphaSetter(MainTilemap, 0f);
        }
    }

    #endregion

    #region Tilemap management

    //private void TileMapDisabler(GameObject go,bool state)
    //{
    //    go.
    //}
    private void TileMapColorAlphaSetter(Tilemap tilemap, float alphaValue)
    {
        Color tilemapNewColor = tilemap.color;

        tilemapNewColor.a = alphaValue;

        tilemap.color = tilemapNewColor;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(BuildingData _buildingData)
    {
        if (temp == null)
        {
            temp = Instantiate(_buildingData.ProductPrefab, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            Building building = temp.GetComponentInChildren<Building>();
            building.BuildingData = _buildingData;
            FollowBuilding();

            TileMapColorAlphaSetter(MainTilemap, 0.5f);
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    #endregion
}
