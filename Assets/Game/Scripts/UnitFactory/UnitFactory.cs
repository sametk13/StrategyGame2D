using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoSingleton<UnitFactory>
{
    // Dictionary to store the unit templates
    private Dictionary<UnitType, UnitData> unitDatas;

    void Start()
    {
        // Initialize the dictionary
        unitDatas = new Dictionary<UnitType, UnitData>();

        // Load the unit templates from the Resources folder
        UnitData[] datas = Resources.LoadAll<UnitData>(@"Data\Unit");

        // Add the unit templates to the dictionary
        foreach (UnitData data in datas)
        {
            unitDatas.Add(data.unitType, data);
        }
    }

    // Method to spawn a unit of the specified type
    public GameObject SpawnUnit(UnitType type, Vector3 position, Quaternion rotation)
    {
        // Check if the unit type exists in the dictionary
        if (unitDatas.ContainsKey(type))
        {
            // If it does, instantiate the unit and return it
            UnitData data = unitDatas[type];
            GameObject unit = Instantiate(data.ProductPrefab, position, rotation);
            return unit;
        }
        else
        {
            // If the unit type does not exist, return null
            Debug.LogError("Unit type " + type + " does not exist in the factory!");
            return null;
        }
    }
}

