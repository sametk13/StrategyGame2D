using SKUtils.Feedbacks;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    [SerializeField] Material defaultMaterial;

    public List<Unit> selectedUnitList;
    private ISelectable priorSelected;

    PointerEventData m_PointerEventData;

    [SerializeField] BuildingData barrackData;

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

        List<UnitCounts> unitCounts = new List<UnitCounts>();

        foreach (var unitData in barrackData.UnitDatas)
        {
            unitCounts.Add(new UnitCounts(unitData, 0));
        }

        foreach (var selectedUnit in selectedUnitList)
        {
            foreach (var _unitCounts in unitCounts)
            {
                if (_unitCounts.unitData.unitType == selectedUnit.unitData.unitType)
                {
                    _unitCounts.count++;
                    break;
                }
            }
        }


        //bool initialized = true;
        //for (int i = 0; i < selectedUnitList.Count; i++)
        //{
        //    if (unitCounts.Count < selectedUnitList.Count && initialized)
        //    {
        //        unitCounts.Add(new UnitCounts(selectedUnitList[i], 1));
        //        initialized = false;
        //    }
        //    else
        //    {
        //        if (selectedUnitList.Contains(unitCounts[i - 1].unit) && unitCounts[i - 1].unit.unitData.unitType == selectedUnitList[i].unitData.unitType)
        //        {
        //            Debug.Log("In");
        //            unitCounts[i - 1].count++;
        //        }
        //        else
        //        {
        //            unitCounts.Add(new UnitCounts(selectedUnitList[i], 1));
        //        }
        //    }
        //}

        foreach (var _unitCount in unitCounts)
        {
            Debug.Log(_unitCount.unitData.unitType + " " + _unitCount.count);
            productInfoDatas.Add(new ProductInfoDatas(_unitCount.unitData, _unitCount.count));
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

public class UnitCounts
{
    public UnitCounts(UnitData _unitData, int _count)
    {
        this.unitData = _unitData;
        this.count = _count;
    }
    public UnitData unitData;
    public int count;
}
