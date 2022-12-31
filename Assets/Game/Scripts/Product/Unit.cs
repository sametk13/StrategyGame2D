using UnityEngine;

public class Unit : Product
{
    //Inheriting Unit from product
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    public bool isSelected { get => _isSelected; set => _isSelected = value; }

    private UnitData _unitData;
    private bool _isSelected;

    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public override void Selected()
    {
        isSelected = true;

        spriteRenderer.material = _unitData.outlineMat;
    }

    public override void UnSelected()
    {
        isSelected = false;

        spriteRenderer.material = _unitData.defaultMat;
    }
}
