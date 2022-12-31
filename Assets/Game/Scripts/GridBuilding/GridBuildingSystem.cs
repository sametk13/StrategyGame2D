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

    private static Dictionary<TileType, TileBase> _tileBases = new Dictionary<TileType, TileBase>();

    public GridLayout gridLayout { get => _gridLayout; set => _gridLayout = value; }
    public Tilemap mainTilemap { get => _mainTilemap; set => _mainTilemap = value; }
    public Tilemap tempTilemap { get => _tempTilemap; set => _tempTilemap = value; }


    private GridLayout _gridLayout;
    private Tilemap _mainTilemap;
    private Tilemap _tempTilemap;

    private Building _temp;
    private Vector3 _prevPos;
    private BoundsInt _prevArea;

    #region Unity Methods

    private void Start()
    {
        string tilePath = @"Tiles\";
        _tileBases.Add(TileType.Empty, null);
        _tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        _tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        _tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));

        TileMapColorAlphaSetter(mainTilemap, 0f);
    }

    private void Update()
    {
        if (!_temp)
        {
            return;
        }


        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        if (!_temp.placed)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            if (_prevPos != cellPos)
            {
                _temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos
                    + new Vector3(.5f, .5f, 0f));
                _prevPos = cellPos;
                FollowBuilding();
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && _temp.CanBePlaced())
        {
            _temp.Place();
            OnBuild?.Invoke(_temp);
            Builded?.Invoke();
            _temp = null;

            TileMapColorAlphaSetter(mainTilemap, 0f);
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            OnCancel?.Invoke(_temp);
            Canceled?.Invoke();

            ClearArea();
            Destroy(_temp.gameObject);

            TileMapColorAlphaSetter(mainTilemap, 0f);
        }
    }

    #endregion

    #region Tilemap management

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
            arr[i] = _tileBases[type];
        }
    }

    #endregion

    #region Building Placement

    public void InitializeWithBuilding(BuildingData _buildingData)
    {
        if (_temp == null)
        {
            _temp = Instantiate(_buildingData.productPrefab, Vector3.zero, Quaternion.identity).GetComponent<Building>();
            Building building = _temp.GetComponentInChildren<Building>();
            building.buildingData = _buildingData;
            FollowBuilding();

            TileMapColorAlphaSetter(mainTilemap, 0.5f);
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[_prevArea.size.x * _prevArea.size.y * _prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        tempTilemap.SetTilesBlock(_prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        _temp.area.position = gridLayout.WorldToCell(_temp.gameObject.transform.position);
        BoundsInt buildingArea = _temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, mainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == _tileBases[TileType.White])
            {
                tileArray[i] = _tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        tempTilemap.SetTilesBlock(buildingArea, tileArray);
        _prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);
        foreach (var b in baseArray)
        {
            if (b != _tileBases[TileType.White])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, tempTilemap);
        SetTilesBlock(area, TileType.Green, mainTilemap);
    }

    #endregion
}
