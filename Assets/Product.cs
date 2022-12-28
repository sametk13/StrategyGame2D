using SKUtils.ScriptableSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Product : MonoBehaviour
{
    public ScriptableActionGameObject OnInitializeWithBuilding;
    [SerializeField] TextMeshProUGUI productNameText;
    [SerializeField] Image productImage;
    [SerializeField] Button productButton;
    [SerializeField] ProductData productData;


    public void InitializeItem(ProductData _productData)
    {
        productData = _productData;
        productNameText.SetText(_productData.ProductName);
        productImage.sprite = _productData.ProductSprite;
        productButton.onClick.AddListener(InitializeWithBuilding);
    }
    public void InitializeWithBuilding()
    {
        OnInitializeWithBuilding.CallAction(productData);
    }
}
