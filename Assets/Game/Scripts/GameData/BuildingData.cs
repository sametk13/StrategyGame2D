using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Building/BuildingData")]
public class BuildingData : ProductData
{
    public List<UnitData> unitDatas = new List<UnitData>();
}
