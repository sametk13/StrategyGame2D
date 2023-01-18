using UnityEngine;
using UnityEngine.InputSystem;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private Transform _selectionAreaTransform;

    private Vector3 _startPosition;

    private void Awake()
    {
        _selectionAreaTransform.gameObject.SetActive(false);
    }
    //Handling box selection on update
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _selectionAreaTransform.gameObject.SetActive(true);
            _startPosition = GetMouseWorldPosition();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 currentMousePosition = GetMouseWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(_startPosition.x, currentMousePosition.x),
                Mathf.Min(_startPosition.y, currentMousePosition.y));
            Vector3 upperRight = new Vector3(
                Mathf.Max(_startPosition.x, currentMousePosition.x),
                Mathf.Max(_startPosition.y, currentMousePosition.y));
            _selectionAreaTransform.position = lowerLeft;
            _selectionAreaTransform.localScale = upperRight - lowerLeft;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(_startPosition, GetMouseWorldPosition());

            ProductSelectManager.Instance.ClearSelectedUnits();

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
