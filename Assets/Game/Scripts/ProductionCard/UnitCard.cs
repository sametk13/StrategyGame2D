using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : ProductCard
{
    public UnitData unitData { get => _unitData; set => _unitData = value; }
    private UnitData _unitData;

    public override void Indicate()
    {
        //Factory
    }

    public override void InitializeCard(UnitData _unitData)
    {
        this.unitData = _unitData;
        ProductNameText.SetText(unitData.ProductName);
        ProductImage.sprite = unitData.ProductSprite;
        ProductButton.onClick.AddListener(Indicate);
    }
}
