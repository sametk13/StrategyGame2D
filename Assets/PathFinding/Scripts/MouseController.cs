using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.InputSystem;

    public class MouseController : MonoBehaviour
    {
        public float speed;
        private Unit unit;

        public OverlayTile standingOnTile;
        OverlayTile priorTile;

        private PathFinder pathFinder;
        private List<OverlayTile> path;

        private List<OverlayTile> overlayTiles;
        private bool isMoving;


        private void Start()
        {

            unit = GetComponent<Unit>();

            pathFinder = new PathFinder();

            path = new List<OverlayTile>();

            overlayTiles = MapManager.Instance.GetSurroundingTiles();

            Vector2Int tileToCheck = new Vector2Int((int)transform.position.x, (int)transform.position.y);

            standingOnTile = MapManager.Instance.GetStandingOnTile(tileToCheck);
            isMoving = false;
        }

        void LateUpdate()
        {
            if (unit.IsSelected || isMoving)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    RaycastHit2D? hit = GetFocusedOnTile();
                    if (hit == null) return;

                    OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                    if (tile == null) return;

                    if (overlayTiles.Contains(tile))
                    {
                        path = pathFinder.FindPath(standingOnTile, tile, overlayTiles);

                        for (int i = 0; i < path.Count; i++)
                        {
                            var previousTile = i > 0 ? path[i - 1] : standingOnTile;
                            var futureTile = i < path.Count - 1 ? path[i + 1] : null;
                        }
                    }
                }

                if (path.Count > 0)
                {
                    isMoving = true;
                    MoveAlongPath();
                }
                else
                {

                    isMoving = false;
                }
            }
        }

        private void MoveAlongPath()
        {
            if (path[0].isBlocked)
            {
                path = pathFinder.FindPath(standingOnTile, standingOnTile, overlayTiles);
                return;
            }

            var step = speed * Time.deltaTime;

            float zIndex = path[0].transform.position.z;
            transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
            transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

            if (Vector2.Distance(transform.position, path[0].transform.position) < 0.00001f)
            {
                PositionCharacterOnLine(path[0]);
                path.RemoveAt(0);
            }
        }

        private void PositionCharacterOnLine(OverlayTile tile)
        {
            transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
            GetComponentInChildren<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            standingOnTile = tile;
            standingOnTile.isBlocked = true;
            if (priorTile != null) priorTile.isBlocked = false;
            priorTile = standingOnTile;
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

