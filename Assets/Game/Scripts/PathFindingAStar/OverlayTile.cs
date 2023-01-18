using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    //Overlay Tile Property Definitor
    public Vector3Int gridLocation { get ; set; }
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
    public bool isBlocked { get ; set ; }
    public OverlayTile previous { get ; set ; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }


    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        HideTile();
    }

    public void SetTileColor(Color _color)
    {
        _spriteRenderer.color = _color;
    }

    public void HideTile()
    {
        _spriteRenderer.enabled = false;
    }

    public void ShowTile()
    {
        _spriteRenderer.enabled = true;
    }
}

