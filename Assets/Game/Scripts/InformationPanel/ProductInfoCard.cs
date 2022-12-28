using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductInfoCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI productNameText;
    [SerializeField] TextMeshProUGUI productCountText;
    [SerializeField] Image productImage;

    public void InitializeInfoCard(ProductData _productData, int count = 1)
    {
        productNameText.SetText(_productData.ProductName);
        productCountText.SetText(count.ToString());
        productImage.sprite = _productData.ProductSprite;

    }
}
