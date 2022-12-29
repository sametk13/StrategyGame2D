using SKUtils.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    [SerializeField] Material defaultMaterial, outlineMaterial;

    private SpriteRenderer prevSelectedObject;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (prevSelectedObject != null)
                prevSelectedObject.material = defaultMaterial;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider == null) return;

            ISelectable selectable = hit.collider.GetComponentInChildren<ISelectable>();
            if (selectable != null)
            {
                selectable.Selected();
                SetSelectedObjectMaterial(hit.transform.gameObject);
                PlaySelectedObjectPunchScale(hit.transform.gameObject);

                Debug.Log("Target object: " + hit.collider.gameObject.transform.name, hit.collider.gameObject);
            }
        }
    }

    private void SetSelectedObjectMaterial(GameObject go)
    {
        SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
        renderer.material = outlineMaterial;
        prevSelectedObject = renderer;
    }
    private void PlaySelectedObjectPunchScale(GameObject go)
    {
        PunchScaleFeedBack punchScaleFeedBack = go.GetComponentInChildren<PunchScaleFeedBack>();
        punchScaleFeedBack.PunchScale();
    }
}
