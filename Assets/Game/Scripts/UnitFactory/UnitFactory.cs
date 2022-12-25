
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public enum UnitEnum
    {
        soldier =0,
        archer
    }

    public Unit UnitSpawner(UnitEnum unitEnum)
    {
        Unit unit = null;

        switch (unitEnum)
        {
            case UnitEnum.soldier:
                unit = new Soldier();
                break;
            case UnitEnum.archer:
                unit = new Archer();
                break;
        }
        return unit;
    }

    public void TestFactory(int unitIndex)
    {
        Unit unit = UnitSpawner((UnitEnum)unitIndex);
        unit.Spawn();
    }
}
