using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Building/BuildingData")]
public class ProductData : ScriptableObject
{
    public GameObject ProductPrefab;
    public string ProductName;
    public Sprite ProductSprite;
    public Vector2Int CellSize;
    public Material outlineMat, defaultMat;
}
