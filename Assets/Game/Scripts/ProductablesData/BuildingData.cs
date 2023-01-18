using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/BuildingData")]
public class BuildingData : ProductData
{
    [SerializeField]private BuildingType buildingType;
    public List<UnitData> unitDatas = new List<UnitData>();

    public override Enum type { get => buildingType; set => buildingType = (BuildingType)value; }
}
