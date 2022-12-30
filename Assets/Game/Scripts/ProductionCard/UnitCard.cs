using UnityEngine;

public class UnitCard : ProductCard
{
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    private UnitData _unitData;
    [HideInInspector] public Vector2 spawnPoint;

    public override void Indicate()
    {
        Unit unit = UnitFactory.Instance.SpawnUnit(unitData.unitType, spawnPoint, Quaternion.identity).GetComponent<Unit>();
        unit.unitData = _unitData;
    }

    public override void InitializeCard(UnitData _unitData)
    {
        this.unitData = _unitData;
        ProductNameText.SetText(unitData.ProductName);
        ProductImage.sprite = unitData.ProductSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
