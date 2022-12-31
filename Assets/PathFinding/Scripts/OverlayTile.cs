using System.Collections.Generic;
using UnityEngine;

    public class OverlayTile : MonoBehaviour
    {
        public int G;
        public int H;
        public int F { get { return G + H; } }

        public bool isBlocked = false;

        public OverlayTile Previous;
        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation {get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

        public List<Sprite> arrows;



        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }

        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

