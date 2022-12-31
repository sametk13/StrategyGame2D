using UnityEngine;

public class BuildingCard : ProductCard
{
    public BuildingData buildingData { get => _buildingData; set => _buildingData = value; }
    private BuildingData _buildingData;
    public override void Indicate() //Card Indication
    {
        GridBuildingSystem.Instance.InitializeWithBuilding(buildingData);
    }

    public override void InitializeCard(BuildingData _buildingData) //Initialization of Card
    {
        this.buildingData = _buildingData;
        ProductNameText.SetText(buildingData.productName);
        ProductImage.sprite = buildingData.productSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
