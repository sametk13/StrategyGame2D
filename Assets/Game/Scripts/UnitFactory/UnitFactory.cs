using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoSingleton<UnitFactory>
{
    private Dictionary<UnitType, UnitData> _unitDatas;

    void Start()
    {
        _unitDatas = new Dictionary<UnitType, UnitData>();

        UnitData[] datas = Resources.LoadAll<UnitData>(@"Data\Unit");

        foreach (UnitData data in datas)
        {
            _unitDatas.Add(data.unitType, data);
        }
    }

    public GameObject SpawnUnit(UnitType type, OverlayTile spawnTile, OverlayTile targetTile)
    {
        //Handling spawn if the unit exist in the dictionary
        if (_unitDatas.ContainsKey(type))
        {
            UnitData data = _unitDatas[type];
            GameObject unit = Instantiate(data.productPrefab, spawnTile.gridLocation,Quaternion.identity);
            UnitPathFinderController unitPathFinderController = unit.GetComponent<UnitPathFinderController>();

            //unitPathFinderController.MoveToTile(targetTile);

            return unit;
        }
        else
        {
            return null;
        }
    }
}