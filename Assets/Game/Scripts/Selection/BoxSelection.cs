using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private Transform selectionAreaTransform;

    private Vector3 startPosition;

    private void Awake()
    {
        selectionAreaTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = GetMouseWorldPosition();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y));
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, GetMouseWorldPosition());

            ProductSelectManager.Instance.ClearSelectionUnitList();

            foreach (Collider2D collider2D in collider2DArray)
            {
                Unit unit = collider2D.GetComponent<Unit>();
                if (unit != null)
                {
                    ProductSelectManager.Instance.AddSelectionUnit(unit);
                    unit.Selected();
                    Debug.Log(unit.gameObject.name);
                }
            }
            ProductSelectManager.Instance.AppendUnitInfoList();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
