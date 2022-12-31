using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Product
{
    //Inheriting a product to a building
    public BuildingData BuildingData { get => _buildingData; set => InitializeAreaSize(value); }
    public bool placed { get => _placed; private set => _placed = value; }
    public Transform spawnPoint { get => _spawnPoint; private set => _spawnPoint = value; }
    public Vector2 nextSpawnPoint { get => _nextSpawnPoint; private set => _nextSpawnPoint = value; }
    public BoundsInt area;


    private bool _placed;
    private BuildingData _buildingData;
    [SerializeField] private Transform _spawnPoint;
    private Vector2 _nextSpawnPoint;
    [SerializeField] private BuildingData buildingData;

    private void Start()
    {
        nextSpawnPoint = spawnPoint.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void InitializeAreaSize(BuildingData _buildingData)
    {
        this.buildingData = _buildingData;

        area.size = new Vector3Int(
            _buildingData.cellSize.x,
            _buildingData.cellSize.y,
            1);
    }

    public override void Selected() //Selecting Building
    {
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(buildingData, 1));
        //Handling Panels
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);

        ProductionMenuHandler.Instance.SetProductCardList(buildingData.unitDatas, ProductType.Unit, this);

        spriteRenderer.material = buildingData.outlineMat;
    }

    public override void UnSelected()
    {
        spriteRenderer.material = buildingData.defaultMat;
    }
    public bool CanBePlaced() //Check if it is placeable
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GridBuildingSystem.Instance.CanTakeArea(areaTemp))
        {
            return true;
        }

        return false;
    }

    public void Place()//Placement
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        placed = true;
        GridBuildingSystem.Instance.TakeArea(areaTemp);
    }
}
