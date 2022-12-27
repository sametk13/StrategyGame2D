using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoSingleton<GridBuildingSystem>
{
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    #region Unity Methods

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    #endregion

    #region Tilemap Management

    #endregion

    #region Building Placement

    #endregion
}

public enum TileType
{
    Empty,
    White,
    Green,
    Red
}
