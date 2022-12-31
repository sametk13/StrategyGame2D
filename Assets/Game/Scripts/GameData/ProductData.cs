using UnityEngine;

[CreateAssetMenu(menuName = "Building/BuildingData")]
public class ProductData : ScriptableObject
{
    public GameObject productPrefab;
    public string productName;
    public Sprite productSprite;
    public Vector2Int cellSize;
    public Material outlineMat, defaultMat;
}
