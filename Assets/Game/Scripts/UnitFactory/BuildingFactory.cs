using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : IProductFactory
{
    private Dictionary<BuildingType, BuildingData> _buildingDataPairs;

    private BuildingType _buildingType;

    public BuildingFactory(BuildingType buildingType, BuildingFactoryDatas factoryDatas)
    {
        _buildingType = buildingType;

        _buildingDataPairs = new Dictionary<BuildingType, BuildingData>();

        foreach (BuildingData data in factoryDatas.buildingDatas)
        {
            _buildingDataPairs.Add((BuildingType)data.type, data);
        }
    }

    public GameObject CreateProduct()
    {
        if (_buildingDataPairs.ContainsKey(_buildingType))
        {
            BuildingData data = _buildingDataPairs[_buildingType];

            GameObject newBuilding = Object.Instantiate(data.productPrefab);

            Building building = newBuilding.GetComponent<Building>();

            GridBuildingSystem.Instance.InitializeBuilding(building);
            building.InitalizeBuilding(data);

            return newBuilding;
        }
        else
        {
            return null;
        }
    }
}
