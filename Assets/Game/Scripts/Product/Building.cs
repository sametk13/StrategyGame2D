﻿using System;
using UnityEngine;

public abstract class Building : MonoBehaviour, IProduct
{
    public virtual BuildingData buildingData { get; private set; }
    public bool isPlaced { get; private set; }
    public BoundsInt area;

    //Implementing Unit from Iproduct
    public Action OnSelect { get; set; }
    public Action OnUnSelect { get; set; }
    public bool isSelected { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    private Collider2D _collider;

    public void InitalizeBuilding(BuildingData buildingData)
    {
        this.buildingData = buildingData;
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        InitializeAreaSize();
    }
    private void InitializeAreaSize()
    {
        area.size = new Vector3Int(buildingData.cellSize.x, buildingData.cellSize.y, 1);
    }
    public virtual void build()//Placement
    {
        Vector3Int positionInt = GridMapManager.Instance.GetNearestOnTile(transform.position).gridLocation;
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        isPlaced = true;
        EnableCollider();
    }

    public virtual void Selected()
    {
        OnSelect?.Invoke();
        isSelected = true;
        spriteRenderer.material = buildingData.outlineMat;
    }

    public virtual void UnSelected()
    {
        OnUnSelect?.Invoke();
        isSelected = false;
        spriteRenderer.material = buildingData.defaultMat;
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }
    public void DissableCollider()
    {
        _collider.enabled = false;
    }

}
