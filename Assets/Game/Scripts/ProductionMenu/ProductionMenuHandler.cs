using SKUtils.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductionMenuHandler : MonoSingleton<ProductionMenuHandler>
{
    public static Action OnProductionChange;
    public static UnityEvent ProductionChanged;

    public List<BuildingData> buildingDatas = new List<BuildingData>();


    [SerializeField] private Transform _scrollContent;
    [SerializeField] private GameObject _buildingCardPrefab;
    [SerializeField] private GameObject _unitCardPrefab;
    private List<GameObject> _currentProducts = new List<GameObject>();

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
        for (int i = 0; i < _currentProducts.Count; i++)
        {
            _currentProducts[i].transform.parent = null;
            _currentProducts[i].GetComponentInChildren<Button>().onClick.RemoveAllListeners();

            if (_currentProducts[i].GetComponent<UnitCard>())
            {
                ObjectPoolManager.Instance.AddObject("unitCard", _currentProducts[i]);
            }
            else if (_currentProducts[i].GetComponent<BuildingCard>())
            {
                ObjectPoolManager.Instance.AddObject("buildingCard", _currentProducts[i]);
            }
        }
        _currentProducts.Clear();
    }

    public GameObject GetProductPrefab(ProductType _productType)
    {
        GameObject productPrefab = null;

        switch (_productType)
        {
            case ProductType.Building:
                productPrefab = _buildingCardPrefab;
                break;
            case ProductType.Unit:
                productPrefab = _unitCardPrefab;
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
                newProduct.transform.parent = _scrollContent;
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), _scrollContent).GetComponent<BuildingCard>();
            }
            _currentProducts.Add(newProduct.gameObject);
            BuildingData currentData = _buildingDatas[i];
            newProduct.InitializeCard(currentData);
        }
        OnProductionChange?.Invoke();
        ProductionChanged?.Invoke();
    }
    public void SetProductCardList(List<UnitData> _unitDatas, ProductType _productType, Building _building)
    {
        ClearProducts();//Clear first

        for (int i = 0; i < _unitDatas.Count; i++)
        {
            UnitCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("unitCard"); //Pooling
            if (product != null)
            {
                newProduct = product.GetComponent<UnitCard>();
                newProduct.transform.parent = _scrollContent;
            }
            else
            {
                newProduct = Instantiate(GetProductPrefab(_productType), _scrollContent).GetComponent<UnitCard>();
            }
            _currentProducts.Add(newProduct.gameObject);
            UnitData currentData = _unitDatas[i];
            newProduct.InitializeCard(currentData);
            newProduct.spawnPoint = _building.spawnPoint.position;
        }
        OnProductionChange?.Invoke();
    }
}
