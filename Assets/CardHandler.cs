using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public TextMeshProUGUI ProductNameText;
    public Image ProductImage;
    public Button ProductButton;

    Enum _productType;

    public  void Indicate()
    {
        if (_productType is UnitType)
        {
            IProductFactory productFactory = new UnitFactory((UnitType)_productType, ProductionMenuHandler.Instance.unitFactoryDatas);
            productFactory.CreateProduct();
        }
        else if (_productType is BuildingType)
        {
            IProductFactory productFactory = new BuildingFactory((BuildingType)_productType, ProductionMenuHandler.Instance.buildingFactory);
            productFactory.CreateProduct();
        }
    }

    public  void InitializeCard(Enum type,string productName, Sprite productSprite)
    {
        _productType = type;
        ProductNameText.SetText(productName);
        ProductImage.sprite = productSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
