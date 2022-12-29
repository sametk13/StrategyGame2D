﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, ISelectable
{
    public bool Placed { get; private set; }
    [HideInInspector] public BoundsInt area;

    public BuildingData BuildingData  { get => buildingData;  set => InitializeAreaSize(value); }
    [SerializeField]private BuildingData buildingData;

    private void InitializeAreaSize(BuildingData _buildingData)
    {
        this.buildingData = _buildingData;

        area.size = new Vector3Int(
            _buildingData.CellSize.x,
            _buildingData.CellSize.y,
            1);
    }

    public void Selected()
    {
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(BuildingData, 1));
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);


        ProductionMenuHandler.Instance.SetProductCardList(BuildingData.UnitDatas, ProductType.unit);
    }
    #region Build Methods

    public bool CanBePlaced()
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

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.Instance.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.Instance.TakeArea(areaTemp);
    }



    #endregion
}
