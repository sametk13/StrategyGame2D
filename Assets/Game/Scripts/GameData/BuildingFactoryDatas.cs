using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ProductFactory/BuildingFactoryDatas")]
public class BuildingFactoryDatas : ScriptableObject
{
    public List<BuildingData> buildingDatas = new List<BuildingData>();
    public void Init(List<BuildingData> buildingDatas)
    {
        this.buildingDatas = buildingDatas;
    }
    public static BuildingFactoryDatas CreateInstance(List<BuildingData> buildingDatas)
    {
        var data = ScriptableObject.CreateInstance<BuildingFactoryDatas>();
        data.Init(buildingDatas);
        return data;
    }
}
