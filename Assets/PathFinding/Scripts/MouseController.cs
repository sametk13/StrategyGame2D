using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static finished3.ArrowTranslator;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.InputSystem;

namespace finished3
{
    public class MouseController : MonoBehaviour
    {
        public float speed;
        private Unit unit;

        public OverlayTile standingOnTile;

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
        //foreach (var item in rangeFinderTiles)
        //   {
        //       item.ShowTile();
        //   }

        void LateUpdate()
        {
            if (!unit.IsSelected || isMoving)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    Debug.Log("test");
                    RaycastHit2D? hit = GetFocusedOnTile();
                    if (hit == null) return;
                    Debug.Log("test1");

                    OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();

                    if (overlayTiles.Contains(tile))
                    {
                        Debug.Log("test2");

                        path = pathFinder.FindPath(standingOnTile, tile, overlayTiles);

                        for (int i = 0; i < path.Count; i++)
                        {
                            var previousTile = i > 0 ? path[i - 1] : standingOnTile;
                            var futureTile = i < path.Count - 1 ? path[i + 1] : null;
                        }
                    }
                    tile.ShowTile();
                    tile.gameObject.GetComponent<OverlayTile>().HideTile();
                }

                if (path.Count > 0)
                {
                    Debug.Log("test3");

                    MoveAlongPath();
                }
            }
        }

        private void MoveAlongPath()
        {
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
}
