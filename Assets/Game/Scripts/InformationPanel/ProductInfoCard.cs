using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductInfoCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI productNameText;
    [SerializeField] TextMeshProUGUI productCountText;
    [SerializeField] Image productImage;

    //Info Card Initialization
    public void InitializeInfoCard(ProductData productData, int count = 1)
    {
        productNameText.SetText(productData.productName);
        productCountText.SetText(count.ToString());
        productImage.sprite = productData.productSprite;
    }
}
