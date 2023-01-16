using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : IProductFactory
{
    private Dictionary<UnitType, UnitData> _unitDataPairs;

    private UnitType _unitType;

    Barrack _barrack;

    public UnitFactory(UnitType unitType, UnitFactoryDatas factoryDatas, Barrack barrack)
    {
        _barrack = barrack;

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

            GameObject newBuilding = Object.Instantiate(data.productPrefab, _barrack.spawnPointTransform.position,Quaternion.identity);

            Unit unit = newBuilding.GetComponent<Unit>();
            unit.InitalizeUnit(data, _barrack);

            return newBuilding;
        }
        else
        {
            return null;
        }
    }
}
