using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Target object: " + hit.collider.gameObject.transform.name, hit.collider.gameObject);
            }
        }
    }
}
