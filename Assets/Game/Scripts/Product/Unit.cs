using finished3;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    private UnitData _unitData;

    public SpriteRenderer spriteRenderer { get; set; }

    public bool IsSelected { get; private set; }

    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public virtual void Selected()
    {
        IsSelected = true;

        spriteRenderer.material = _unitData.outlineMat;
    }

    public virtual void UnSelected()
    {
        IsSelected = false;

        spriteRenderer.material = _unitData.defaultMat;
    }
}
