using SKUtils.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InformationPanelHandler : MonoSingleton<InformationPanelHandler>
{
    public static Action OnInformationListChange;

    private List<GameObject> currentInfoCards = new List<GameObject>();

    [SerializeField] private GameObject informationProductPrefab;
    [SerializeField] private Transform layout;


    public void ClearInformationList()
    {
        for (int i = 0; i < currentInfoCards.Count; i++)
        {
            ObjectPoolManager.Instance.AddObject("informationProduct", currentInfoCards[i]);
        }
        currentInfoCards.Clear();
    }
    public void SetInformationList(List<ProductInfoDatas> productInfoDatas)
    {

        ClearInformationList();

        for (int i = 0; i < productInfoDatas.Count; i++)
        {
            ProductInfoCard newProduct;

            GameObject infoCard = ObjectPoolManager.Instance.GetObject("informationProduct");
            if (infoCard != null)
            {
                newProduct = infoCard.GetComponent<ProductInfoCard>();
            }
            else
            {
                newProduct = Instantiate(informationProductPrefab, layout).GetComponent<ProductInfoCard>();
            }
            currentInfoCards.Add(newProduct.gameObject);
            ProductInfoDatas currentData = productInfoDatas[i];
            newProduct.InitializeInfoCard(currentData.productData, currentData.count);
        }
        OnInformationListChange?.Invoke();
    }
}