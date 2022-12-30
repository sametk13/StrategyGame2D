using SKUtils.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

public class InformationPanelHandler : MonoSingleton<InformationPanelHandler>
{
    [SerializeField] private GameObject informationProductPrefab;
    [SerializeField] private Transform layout;

    private List<GameObject> currentInfoCards = new List<GameObject>();

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
            newProduct.InitializeInfoCard(currentData.ProductData, currentData.Count);
        }
    }
}
[System.Serializable]
public class ProductInfoDatas
{
    public ProductInfoDatas(ProductData _productData, int _count)
    {
        this.ProductData = _productData;
        this.Count = _count;
    }

    public ProductData ProductData;
    public int Count;
}
