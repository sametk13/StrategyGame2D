using System.Collections.Generic;
using UnityEngine;

public class UnitMovementHandler : MonoBehaviour
{
    public float speed { get => _speed; set => _speed = value; }
    public OverlayTile standingOnTile { get; set; }


    [SerializeField] private float _speed;
    private PathFinder _pathFinder;
    private List<OverlayTile> _path;
    private OverlayTile _previousEndTile;

    private void Awake()
    {
        _pathFinder = new PathFinder();

        _path = new List<OverlayTile>();

        standingOnTile = GridMapManager.Instance.GetNearestOnTile(transform.position);
    }

    void LateUpdate()
    {
        if (_path != null && _path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void RemovePreviousTile()
    {
        if (_previousEndTile != null)
        {
            _previousEndTile.isBlocked = false;
            _previousEndTile = null;
        }
    }

    public void MoveToTile(OverlayTile targetTile)
    {

        RemovePreviousTile();
        //Finding Path
        _path = _pathFinder.FindPath(standingOnTile, targetTile, ref _previousEndTile);

        for (int i = 0; i < _path.Count; i++)
        {
            var previousTile = i > 0 ? _path[i - 1] : standingOnTile;
            var futureTile = i < _path.Count - 1 ? _path[i + 1] : null;
        }
    }

    private void MoveAlongPath()
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
    }


}

