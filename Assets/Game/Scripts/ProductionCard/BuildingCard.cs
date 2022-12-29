
using UnityEngine;

public class BuildingCard : ProductCard
{
    public BuildingData buildingData { get => _buildingData; set => _buildingData = value; }
    private BuildingData _buildingData;
    public override void Indicate()
    {
        GridBuildingSystem.Instance.InitializeWithBuilding(buildingData);
    }

    public override void InitializeCard(BuildingData _buildingData)
    {
        this.buildingData = _buildingData;
        ProductNameText.SetText(buildingData.ProductName);
        ProductImage.sprite = buildingData.ProductSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
