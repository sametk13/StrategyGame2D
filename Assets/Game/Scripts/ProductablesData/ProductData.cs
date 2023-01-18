using System;
using UnityEngine;

public abstract class ProductData : ScriptableObject
{
    public GameObject productPrefab;
    public string productName;
    public Sprite productSprite;
    public Vector2Int cellSize;
    public Material outlineMat, defaultMat;
    public abstract Enum type { get; set; }
}
