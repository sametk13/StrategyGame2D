using SKUtils.Feedbacks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    [SerializeField] Material defaultMaterial;

    public List<Unit> selectedUnitList;
    private ISelectable priorSelected;

    PointerEventData m_PointerEventData;

    private void Awake()
    {
        selectedUnitList = new List<Unit>();
    }
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider == null) return;
            ISelectable selectable = hit.collider.GetComponentInChildren<ISelectable>();

            if (priorSelected != null && !UILeftClickDetector())
            {
                ClearSelectionUnitList();

                InformationPanelHandler.Instance.ClearInformationList();
                ProductionMenuHandler.Instance.ClearProducts();
                ProductionMenuHandler.Instance.GetBuildingDatas();
            }

            if (selectable != null)
            {
                selectable.Selected();
                PlaySelectedObjectPunchScale(hit.transform.gameObject);
                priorSelected = selectable;
                Debug.Log("Target object: " + hit.transform.gameObject.name);
            }
        }
    }

    public void AppendUnitInfoList()
    {
        if (selectedUnitList.Count == 0) return;

        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();

        foreach (var unit in selectedUnitList)
        {
            productInfoDatas.Add(new ProductInfoDatas(unit.unitData, 1));
        }
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);
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

    private void PlaySelectedObjectPunchScale(GameObject go)
    {
        PunchScaleFeedBack punchScaleFeedBack = go.GetComponentInChildren<PunchScaleFeedBack>();
        punchScaleFeedBack.PunchScale();
    }

    public void AddSelectionUnit(Unit _unit)
    {
        selectedUnitList.Add(_unit);
    }
    public void ClearSelectionUnitList()
    {
        foreach (var unit in selectedUnitList)
        {
            unit.UnSelected();
        }
        selectedUnitList.Clear();
    }
}
