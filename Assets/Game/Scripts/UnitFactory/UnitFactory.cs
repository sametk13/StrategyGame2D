using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : IProductFactory
{
    private Dictionary<UnitType, UnitData> _unitDataPairs;

    private UnitType _unitType;

    public UnitFactory(UnitType unitType, UnitFactoryDatas factoryDatas)
    {
        _unitType = unitType;

        _unitDataPairs = new Dictionary<UnitType, UnitData>();

        foreach (UnitData data in factoryDatas.unitDatas)
        {
            _unitDataPairs.Add((UnitType)data.type, data);
        }
    }

    public GameObject CreateProduct()
    {
        if (_unitDataPairs.ContainsKey(_unitType))
        {
            UnitData data = _unitDataPairs[_unitType];

            GameObject newBuilding = Object.Instantiate(data.productPrefab);

            Unit unit = newBuilding.GetComponent<Unit>();
            unit.InitalizeUnit(data);

            return newBuilding;
        }
        else
        {
            return null;
        }
    }
}
