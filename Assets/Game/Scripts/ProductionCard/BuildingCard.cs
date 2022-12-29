
using UnityEngine;

public class BuildingCard : ProductCard
{
   
    public override void Indicate()
    {
        GridBuildingSystem.Instance.InitializeWithBuilding(ProductData);
    }
}
