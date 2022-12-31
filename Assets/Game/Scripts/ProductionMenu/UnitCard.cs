using UnityEngine;

public class UnitCard : ProductCard
{
    //Holding unitcard data
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    public Vector2 spawnPoint { get => _spawnPoint; set => _spawnPoint = value; }


    private UnitData _unitData;
    private Vector2 _spawnPoint;

    public override void Indicate()
    {
        Unit unit = UnitFactory.Instance.SpawnUnit(unitData.unitType, spawnPoint, Quaternion.identity).GetComponent<Unit>();
        unit.unitData = _unitData;
    }

    public override void InitializeCard(UnitData _unitData)
    {
        this.unitData = _unitData;
        ProductNameText.SetText(unitData.productName);
        ProductImage.sprite = unitData.productSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
