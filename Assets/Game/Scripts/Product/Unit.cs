using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IProduct
{
    public virtual UnitData unitData { get; private set; }

    //Implementing Unit from Iproduct
    public Action OnSelect { get; set; }
    public Action OnUnSelect { get; set; }
    public bool isSelected { get; set; }


    public void InitalizeUnit(UnitData unitData, Barrack baseBarrack)
    {
        this.unitData = unitData;
        GetComponent<UnitMovementHandler>().MoveToTile(baseBarrack.destinationTile);
    }

    public virtual void Selected()
    {
        OnSelect?.Invoke();
        isSelected = true;
    }

    public virtual void UnSelected()
    {
        OnUnSelect?.Invoke();
        isSelected = false;
    }
}
