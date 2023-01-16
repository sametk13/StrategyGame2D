using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Raycaster
{
    public static RaycastHit2D? GetMouseRaycastHit()
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
