using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/UnitData")]
public class UnitData : ProductData
{
    [SerializeField]private UnitType unitType;

    public override Enum type { get => unitType; set => unitType = (UnitType)value; }
}
