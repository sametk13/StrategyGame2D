using UnityEngine;

public class Unit : Product
{
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    private UnitData _unitData;

    public bool IsSelected { get; private set; }

    public virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public override void Selected()
    {
        Debug.Log("Selected Unit");

        IsSelected = true;

        spriteRenderer.material = _unitData.outlineMat;
    }

    public override void UnSelected()
    {
        Debug.Log("UnSelected Unit");

        IsSelected = false;

        spriteRenderer.material = _unitData.defaultMat;
    }
}
