using SKUtils.Feedbacks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ProductSelectManager : MonoSingleton<ProductSelectManager>
{
    public List<Unit> selectedUnitList;

    private ISelectable _priorSelected;
    private PointerEventData _m_PointerEventData;
    [SerializeField] private BuildingData _barrackData;

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

            if (selectable != null && selectable.isSelected) return; 

            if ( _priorSelected != null && !UILeftClickDetector())
            {
                ClearSelectionUnitList();
                _priorSelected.UnSelected();
                _priorSelected = null;
                InformationPanelHandler.Instance.ClearInformationList();
                ProductionMenuHandler.Instance.ClearProducts();
                ProductionMenuHandler.Instance.GetBuildingDatas();
            }

            if (selectable != null)
            {
                selectable.Selected();
                _priorSelected = selectable;
            }
        }
    }

    public void AppendUnitInfoList()
    {
        if (selectedUnitList.Count == 0) return;

        Debug.Log("testt");
        List<ProductInfoDatas> productInfoDatas = new List<ProductInfoDatas>();

        List<UnitCounts> unitCounts = new List<UnitCounts>();

        foreach (var unitData in _barrackData.unitDatas)
        {
            Debug.Log("testt1");

            unitCounts.Add(new UnitCounts(unitData, 0));
        }

        foreach (var selectedUnit in selectedUnitList)
        {
            Debug.Log("testt2");

            foreach (var _unitCounts in unitCounts)
            {
                Debug.Log("testt3");

                if ((UnitType)_unitCounts.unitData.type == (UnitType)selectedUnit.unitData.type)
                {
                    Debug.Log("testt4");

                    _unitCounts.count++;
                    break;
                }
            }
        }

        foreach (var _unitCount in unitCounts)
        {
            Debug.Log("testt5");

            if (_unitCount.count > 0)
            {
                Debug.Log("testt6");

                productInfoDatas.Add(new ProductInfoDatas(_unitCount.unitData, _unitCount.count));
            }
        }
        InformationPanelHandler.Instance.SetInformationList(productInfoDatas);
    }

    private bool UILeftClickDetector()
    {
        //Detecting if the click is on ui or not
        _m_PointerEventData = new PointerEventData(EventSystem.current);
        _m_PointerEventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_m_PointerEventData, results);
        if (results.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public List<Unit> GetSelectedUnits()
    {
        return selectedUnitList;
    }
}

