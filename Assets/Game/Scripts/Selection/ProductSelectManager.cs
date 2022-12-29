using SKUtils.Feedbacks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    [SerializeField] Material defaultMaterial, outlineMaterial;

    private SpriteRenderer prevSelectedObject;

    PointerEventData m_PointerEventData;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if ( prevSelectedObject != null && !UILeftClickDetector())
            {
                prevSelectedObject.material = defaultMaterial;
                InformationPanelHandler.Instance.ClearInformationList();
                ProductionMenuHandler.Instance.ClearProducts();
                ProductionMenuHandler.Instance.GetBuildingDatas();
            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider == null) return;

            ISelectable selectable = hit.collider.GetComponentInChildren<ISelectable>();
            if (selectable != null)
            {
                selectable.Selected();
                SetSelectedObjectMaterial(hit.transform.gameObject);
                PlaySelectedObjectPunchScale(hit.transform.gameObject);
                Debug.Log("Target object: " + hit.transform.gameObject.name);
            }
        }
    }

    private bool UILeftClickDetector()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(EventSystem.current);
        //Interconnecting the data position with the mouse position
        m_PointerEventData.position = Mouse.current.position.ReadValue();
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast using the Graphics Raycaster and mouse click position
        EventSystem.current.RaycastAll(m_PointerEventData, results);

        if (results.Count > 0)
        {
            Debug.Log("This is UI");
            return true;
        }
        else
        {
            Debug.Log("This not UI");
            return false;
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
