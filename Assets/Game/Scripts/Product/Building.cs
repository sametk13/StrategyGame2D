using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : Product
{
    //Inheriting a product to a building
    public BuildingData BuildingData { get => _buildingData; set => InitializeAreaSize(value); }
    public bool placed { get => _placed; private set => _placed = value; }
    public Transform spawnPoint { get => _spawnPoint; private set => _spawnPoint = value; }
    public OverlayTile nextTargetTile;
    public Transform nextTargetPoint;
    public BoundsInt area;


    private bool isSelected = false;
    private bool _placed;
    private BuildingData _buildingData;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private BuildingData buildingData;

    private void Start()
    {
        Vector2Int tileToCheck = new Vector2Int((int)_spawnPoint.position.x, (int)_spawnPoint.position.y);

        nextTargetTile = GridMapManager.Instance.GetStandingOnTile(tileToCheck);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Handling spawn point
        if (Mouse.current.rightButton.wasPressedThisFrame && isSelected)
        {
            RaycastHit2D? hit = GetFocusedOnTile();
            if (hit == null) return;

            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            Debug.Log(tile.name, tile);
            if (tile == null) return;

            nextTargetPoint.position = tile.transform.position;
            nextTargetTile = tile;
            ProductionMenuHandler.Instance.SetProductCardList(buildingData.unitDatas, ProductType.Unit, this);
        }
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
        isSelected = true;
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(buildingData, 1));
        //Handling Panels
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);

        ProductionMenuHandler.Instance.SetProductCardList(buildingData.unitDatas, ProductType.Unit, this);

        spriteRenderer.material = buildingData.outlineMat;

        //Handling spawn point
        SpriteRenderer newTargetPointRenderer = nextTargetPoint.GetComponentInChildren<SpriteRenderer>();
        Color newColor = newTargetPointRenderer.material.color;
        newColor.a = 1f;
        newTargetPointRenderer.color = newColor;
    }

    public override void UnSelected()
    {
        isSelected = false;
        spriteRenderer.material = buildingData.defaultMat;

        //Handling spawn point
        SpriteRenderer newTargetPointRenderer = nextTargetPoint.GetComponentInChildren<SpriteRenderer>();
        Color newColor = newTargetPointRenderer.material.color;
        newColor.a = 0f;
        newTargetPointRenderer.color = newColor;
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

    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}
