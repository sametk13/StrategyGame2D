using SKUtils.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenuHandler : MonoSingleton<ProductionMenuHandler>
{
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
        SetProductCardList(buildingDatas,ProductType.Building);
    }

    public void ClearProducts()
    {
        for (int i = 0; i < currentProducts.Count; i++)
        {
            ObjectPoolManager.Instance.AddObject("product", currentProducts[i]);
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

    public void SetProductCardList(List<BuildingData> _buildingDatas,ProductType _productType)
    {
        ClearProducts();

        for (int i = 0; i < _buildingDatas.Count; i++)
        {
            BuildingCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("buildingCard");
            if (product != null)
            {
                newProduct = product.GetComponent<BuildingCard>();
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), scrollContent).GetComponent<BuildingCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            BuildingData currentData = _buildingDatas[i];
            newProduct.InitializeCard(currentData);
        }
    }

    public void SetProductCardList(List<UnitData> _unitDatas, ProductType _productType,Building _building)
    {
        ClearProducts();

        for (int i = 0; i < _unitDatas.Count; i++)
        {
            UnitCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("unitCard");
            if (product != null)
            {
                newProduct = product.GetComponent<UnitCard>();
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
    }
}
