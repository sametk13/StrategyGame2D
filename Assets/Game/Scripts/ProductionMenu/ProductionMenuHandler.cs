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

    public BuildingFactoryDatas buildingFactory;
    public UnitFactoryDatas unitFactoryDatas;


    [SerializeField] private Transform _scrollContent;
    [SerializeField] private GameObject _buttonCardPrefab;

    private List<GameObject> _currentProducts = new List<GameObject>();

    private void Start()
    {
        GetBuildingDatas();
    }
    public void GetBuildingDatas()
    {
        List<ProductData> productDatas = new List<ProductData>();

        for (int i = 0; i < buildingFactory.buildingDatas.Count; i++)
        {
            productDatas.Add(buildingFactory.buildingDatas[i]);
        } 

        SetProductCardList(productDatas);
    }

    public void ClearProducts()
    {
        for (int i = 0; i < _currentProducts.Count; i++)
        {
            _currentProducts[i].transform.parent = null;
            _currentProducts[i].GetComponentInChildren<Button>().onClick.RemoveAllListeners();

            ObjectPoolManager.Instance.AddObject("buttonCard", _currentProducts[i]);
        }
        _currentProducts.Clear();
    }


    public void SetProductCardList(List<ProductData> _productDatas,Barrack barrack = null)
    {
        ClearProducts();

        for (int i = 0; i < _productDatas.Count; i++)
        {
            CardHandler newCard;

            GameObject product = ObjectPoolManager.Instance.GetObject("buttonCard");
            if (product != null)
            {
                newCard = product.GetComponent<CardHandler>();
                newCard.transform.parent = _scrollContent;
            }
            else
            {
                newCard = Instantiate(_buttonCardPrefab, _scrollContent).GetComponent<CardHandler>();
            }
            _currentProducts.Add(newCard.gameObject);
            ProductData currentData = _productDatas[i];
            newCard.InitializeCard(currentData.type, currentData.productName, currentData.productSprite,barrack);
        }
        OnProductionChange?.Invoke();
        ProductionChanged?.Invoke();
    }
}
