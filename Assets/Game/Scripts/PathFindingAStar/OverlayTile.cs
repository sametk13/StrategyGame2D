using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    //Overlay Tile Property Definitor
    public Vector3Int gridLocation { get => _gridLocation; set => _gridLocation = value; }
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public int G { get => _G; set => _G = value; }
    public int H { get => _H; set => _H = value; }
    public int F { get { return G + H; } }

    public bool isBlocked { get => _isBlocked; set => _isBlocked = value; }
    public OverlayTile previous { get => _previous; set => _previous = value; }

    private int _G;
    private int _H;
    private bool _isBlocked = false;
    private OverlayTile _previous;
    private Vector3Int _gridLocation;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileColor(Color _color)
    {
        _spriteRenderer.color = _color;
    }

    public void HideTile()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void ShowTile()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}

