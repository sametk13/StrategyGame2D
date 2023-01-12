using System.Collections.Generic;
using UnityEngine;

public class UnitPathFinderController : MonoBehaviour
{
    public float speed { get => _speed; set => _speed = value; }
    public OverlayTile standingOnTile;

    [SerializeField] private float _speed;

    private PathFinder _pathFinder;
    private List<OverlayTile> _path;

    private void Awake()
    {
        _pathFinder = new PathFinder();

        _path = new List<OverlayTile>();

        Vector2Int tileToCheck = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        standingOnTile = GridMapManager.Instance.GetStandingOnTile(tileToCheck);
    }

    void LateUpdate()
    {
        if (_path != null && _path.Count > 0)
        {
            MoveAlongPath(_path[0]);
        }
    }
    public void MoveToTile(OverlayTile _targetTile)
    {
        _path = _pathFinder.FindPath(standingOnTile, _targetTile);

        for (int i = 0; i < _path.Count; i++)
        {
            var previousTile = i > 0 ? _path[i - 1] : standingOnTile;
            var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;
        }
    }

    private void MoveAlongPath(OverlayTile targetTile)
    {
        var step = speed * Time.deltaTime;

        //Movement
        transform.position = Vector2.MoveTowards(transform.position, _path[0].transform.position, step);

        if (Vector2.Distance(transform.position, _path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(_path[0]);
            _path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        standingOnTile = tile;
        //standingOnTile.isBlocked = true;
    }


}

