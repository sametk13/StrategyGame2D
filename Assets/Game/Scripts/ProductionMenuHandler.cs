using SKUtils.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenuHandler : MonoSingleton<ProductionMenuHandler>
{
    public static Action OnProductionChange;

    public List<BuildingData> buildingDatas = new List<BuildingData>();

    [SerializeField] private Transform scrollContent;

    [SerializeField] private GameObject buildingCardPrefab;
    [SerializeField] private GameObject unitCardPrefab;

    private List<GameObject> currentProducts = new List<GameObject>();

    private void Start()
    {
        GetBuildingDatas();
    }
    public void GetBuildingDatas()
    {
        SetProductCardList(buildingDatas, ProductType.Building);
    }

    public void ClearProducts()
    {
        for (int i = 0; i < currentProducts.Count; i++)
        {
            currentProducts[i].transform.parent = null;

            if (currentProducts[i].GetComponent<UnitCard>())
            {
                ObjectPoolManager.Instance.AddObject("unitCard", currentProducts[i]);
            }
            else if (currentProducts[i].GetComponent<BuildingCard>())
            {
                ObjectPoolManager.Instance.AddObject("buildingCard", currentProducts[i]);
            }
        }
        currentProducts.Clear();
    }

    public GameObject GetProductPrefab(ProductType _productType)
    {
        GameObject productPrefab = null;

        switch (_productType)
        {
            case ProductType.Building:
                productPrefab = buildingCardPrefab;
                break;
            case ProductType.Unit:
                productPrefab = unitCardPrefab;
                break;
        }
        return productPrefab;
    }

    public void SetProductCardList(List<BuildingData> _buildingDatas, ProductType _productType)
    {
        ClearProducts();

        for (int i = 0; i < _buildingDatas.Count; i++)
        {
            BuildingCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("buildingCard");
            if (product != null)
            {
                newProduct = product.GetComponent<BuildingCard>();
                newProduct.transform.parent = scrollContent;
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), scrollContent).GetComponent<BuildingCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            BuildingData currentData = _buildingDatas[i];
            newProduct.InitializeCard(currentData);
        }
        OnProductionChange?.Invoke();
    }

    public void SetProductCardList(List<UnitData> _unitDatas, ProductType _productType, Building _building)
    {
        ClearProducts();

        for (int i = 0; i < _unitDatas.Count; i++)
        {
            UnitCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("unitCard");
            if (product != null)
            {
                newProduct = product.GetComponent<UnitCard>();
                newProduct.transform.parent = scrollContent;
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), scrollContent).GetComponent<UnitCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            UnitData currentData = _unitDatas[i];
            newProduct.InitializeCard(currentData);
            newProduct.spawnPoint = _building.SpawnPoint.position;
        }
        OnProductionChange?.Invoke();
    }
}
