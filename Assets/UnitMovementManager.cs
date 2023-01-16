using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovementManager : MonoBehaviour
{

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RaycastHit2D? hit = GetMouseRaycastHit();
            if (hit == null) 
                return;

            OverlayTile targetTile = hit.Value.collider.GetComponent<OverlayTile>();
            if (targetTile == null)
            {
                targetTile = GridMapManager.Instance.GetNearestOnTile(hit.Value.point);
            }

            MoveSelectedUnits(targetTile);
        }
    }

    private void MoveSelectedUnits(OverlayTile _overlayTile)
    {
        var selectedUnits = ProductSelectManager.Instance.GetSelectedUnits();

        foreach (var unit in selectedUnits)
        {
            UnitMovementHandler unitPathFinderController = unit.GetComponent<UnitMovementHandler>();
            unitPathFinderController.MoveToTile(_overlayTile);
        }
    }


    private RaycastHit2D? GetMouseRaycastHit()
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
