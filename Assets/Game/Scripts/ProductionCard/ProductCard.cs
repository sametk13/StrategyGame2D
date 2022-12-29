using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProductCard : MonoBehaviour
{
    public TextMeshProUGUI ProductNameText;
    public Image ProductImage;
    public Button ProductButton;
    public virtual void InitializeCard(BuildingData _buildingData) { }
    public virtual void InitializeCard(UnitData _unitData) { }

    public abstract void Indicate();
}
