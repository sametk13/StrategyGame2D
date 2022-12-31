using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class UnitPathFinderController : MonoBehaviour
{
    public float speed { get => _speed; set => _speed = value; }
    public OverlayTile standingOnTile;

    [SerializeField]private float _speed;

    private Unit _unit;
    private OverlayTile _priorTile;
    private PathFinder _pathFinder;
    private List<OverlayTile> _path;
    private List<OverlayTile> _overlayTiles;
    private bool _isMoving;


    private void Start()
    {
        _unit = GetComponent<Unit>();

        _pathFinder = new PathFinder();

        _path = new List<OverlayTile>();

        _overlayTiles = MapManager.Instance.GetSurroundingTiles();

        Vector2Int tileToCheck = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        standingOnTile = MapManager.Instance.GetStandingOnTile(tileToCheck);
        _isMoving = false;
    }

    void LateUpdate()
    {
        if (_unit.isSelected || _isMoving)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {

                RaycastHit2D? hit = GetFocusedOnTile();
                if (hit == null) return;

                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                if (tile == null) return;

                if (_overlayTiles.Contains(tile))
                {
                    Vector2Int tileToCheck = new Vector2Int((int)transform.position.x, (int)transform.position.y);

                    if (standingOnTile == null)
                    {
                        standingOnTile = MapManager.Instance.GetStandingOnTile(tileToCheck);
                    }
                    if (standingOnTile == null) return;

                    _path = _pathFinder.FindPath(standingOnTile, tile, _overlayTiles);

                    for (int i = 0; i < _path.Count; i++)
                    {
                        var previousTile = i > 0 ? _path[i - 1] : standingOnTile;
                        var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;
                    }
                }
            }

            if (_path.Count > 0)
            {
                _isMoving = true;
                MoveAlongPath();
            }
            else
            {
                _isMoving = false;
            }
        }
    }

    private void MoveAlongPath()
    {
        if (_path[0].isBlocked)
        {
            _path = _pathFinder.FindPath(standingOnTile, standingOnTile, _overlayTiles); // Calculating Path
            return;
        }

        var step = speed * Time.deltaTime;

        float zIndex = _path[0].transform.position.z;
        //Movement
        transform.position = Vector2.MoveTowards(transform.position, _path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, _path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(_path[0]);
            _path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponentInChildren<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        standingOnTile = tile;
        standingOnTile.isBlocked = true;
        if (_priorTile != null) _priorTile.isBlocked = false;
        _priorTile = standingOnTile;
    }

    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}

