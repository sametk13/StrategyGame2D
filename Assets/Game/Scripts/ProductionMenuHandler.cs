using SKUtils.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
        SetProductCardList(buildingDatas,ProductType.building);
    }

    private void ClearProducts()
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
            case ProductType.building:
                productPrefab = buildingCardPrefab;
                break;
            case ProductType.unit:
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
            ProductCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("buildingCard");
            if (product != null)
            {
                newProduct = product.GetComponent<ProductCard>();
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), scrollContent).GetComponent<ProductCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            BuildingData currentData = _buildingDatas[i];
            newProduct.InitializeCard(currentData);
        }
    }

    public void SetProductCardList(List<UnitData> _unitDatas, ProductType _productType)
    {
        ClearProducts();

        for (int i = 0; i < _unitDatas.Count; i++)
        {
            ProductCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("unitCard");
            if (product != null)
            {
                newProduct = product.GetComponent<ProductCard>();
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), scrollContent).GetComponent<ProductCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            UnitData currentData = _unitDatas[i];
            newProduct.InitializeCard(currentData);
        }
    }
}

public enum ProductType
{
    building,
    unit
}
