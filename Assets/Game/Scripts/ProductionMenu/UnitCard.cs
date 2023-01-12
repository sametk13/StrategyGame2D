using UnityEngine;

public class UnitCard : ProductCard
{
    //Holding unitcard data
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    public OverlayTile spawnTile { get => _spawnTile; set => _spawnTile = value; }

    public OverlayTile targetPoint;


    private UnitData _unitData;
    private OverlayTile _spawnTile;


    public override void Indicate()
    {
        Unit unit = UnitFactory.Instance.SpawnUnit(unitData.unitType, spawnTile, targetPoint).GetComponent<Unit>();
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
