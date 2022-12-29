using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProductCard : MonoBehaviour
{
    public TextMeshProUGUI ProductNameText;
    public Image ProductImage;
    public Button ProductButton;
    public ProductData ProductData { get; set; }
    [SerializeField] private ProductData productData;
    public virtual void InitializeCard(ProductData _productData)
    {
        ProductData = _productData;
        ProductNameText.SetText(_productData.ProductName);
        ProductImage.sprite = _productData.ProductSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
    public abstract void Indicate();
}
