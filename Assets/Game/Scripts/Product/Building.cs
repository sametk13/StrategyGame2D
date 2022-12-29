using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, ISelectable
{
    public bool Placed { get; private set; }
    [HideInInspector] public BoundsInt area;

    public ProductData ProductData { get => productData;  set => InitializeAreaSize(value); }
    private ProductData productData;

    private void InitializeAreaSize(ProductData _productData)
    {
        this.productData = _productData;

        area.size = new Vector3Int(
            _productData.CellSize.x,
            _productData.CellSize.y,
            1);
    }

    public void Selected()
    {
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();
        productInfoDatas.Add(new ProductInfoDatas(productData, 1));
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);
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
