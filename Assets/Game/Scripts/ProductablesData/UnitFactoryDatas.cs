using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ProductFactory/UnitFactoryDatas")]
public class UnitFactoryDatas : ScriptableObject
{
    public List<UnitData> unitDatas = new List<UnitData>();

    public void Init(List<UnitData> unitDatas)
    {
        this.unitDatas = unitDatas;
    }
    public static UnitFactoryDatas CreateInstance(List<UnitData> unitDatas)
    {
        var data = ScriptableObject.CreateInstance<UnitFactoryDatas>();
        data.Init(unitDatas);
        return data;
    }
}
