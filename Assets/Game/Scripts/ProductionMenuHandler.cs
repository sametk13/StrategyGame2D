using SKUtils.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ProductionMenuHandler : MonoSingleton<ProductionMenuHandler>
{
    public List<ProductData> buildingDatas = new List<ProductData>();

    [SerializeField] private Transform scrollContent;

    [SerializeField] private GameObject buildingCardPrefab;
    [SerializeField] private GameObject unitCardPrefab;

    private List<GameObject> currentProducts = new List<GameObject>();

    private void Start()
    {
        SetBuildingCardList(buildingDatas);
    }
    public void SetBuildingCardList(List<ProductData> productDatas)
    {
        for (int i = 0; i < currentProducts.Count; i++)
        {
            ObjectPoolManager.Instance.AddObject("product", currentProducts[i]);
        }
        currentProducts.Clear();


        for (int i = 0; i < productDatas.Count; i++)
        {
            ProductCard newProduct;

            GameObject product = ObjectPoolManager.Instance.GetObject("product");
            if (product != null)
            {
                newProduct = product.GetComponent<ProductCard>();
            }
            else
            {
                newProduct = Instantiate(buildingCardPrefab, scrollContent).GetComponent<ProductCard>();
            }
            currentProducts.Add(newProduct.gameObject);
            ProductData currentData = productDatas[i];
            newProduct.InitializeCard(currentData);
        }
    }
}
