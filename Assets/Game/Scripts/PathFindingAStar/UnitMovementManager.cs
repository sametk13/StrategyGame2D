using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovementManager : MonoBehaviour
{

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RaycastHit2D? hit = Raycaster.GetMouseRaycastHit();
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

    private void MoveSelectedUnits(OverlayTile overlayTile)
    {
        var selectedUnits = ProductSelectManager.Instance.GetSelectedUnits();

        List<UnitMovementHandler> unitPathFinderControllers = new List<UnitMovementHandler>();
        foreach (var unit in selectedUnits)
        {
            UnitMovementHandler unitMovement = unit.GetComponent<UnitMovementHandler>();
            unitMovement.RemovePreviousTile();
            unitPathFinderControllers.Add(unitMovement);
        }

        foreach (var unit in unitPathFinderControllers)
        {
            unit.MoveToTile(overlayTile);
        }
    }
}
