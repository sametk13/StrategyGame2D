using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{

    [SerializeField]
    private ScrollContent _scrollContent;

    [SerializeField]
    private float _outOfBoundsThreshold;
    private ScrollRect _scrollRect;

    private Vector2 _lastDragPosition;

    private bool _positiveDrag;


    private void OnEnable()
    {
        ProductionMenuHandler.OnProductionChange += StartIEOnViewScroll;
    }

    private void OnDisable()
    {
        ProductionMenuHandler.OnProductionChange -= StartIEOnViewScroll;
    }

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.vertical = true;
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _positiveDrag = eventData.position.y > _lastDragPosition.y;

        _lastDragPosition = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        _positiveDrag = eventData.scrollDelta.y > 0;
    }

    public void StartIEOnViewScroll()
    {
        StartCoroutine(IEOnViewScroll());
    }

    IEnumerator IEOnViewScroll()
    {
        yield return new WaitForSeconds(0.1f);
        OnViewScroll();
    }

    public void OnViewScroll()
    {
        HandleVerticalScroll();
    }

    private void HandleVerticalScroll()
    {
        if (_scrollRect.content.childCount == 0) return;
        int currItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
        var currItem = _scrollRect.content.GetChild(currItemIndex);

        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
        Transform endItem = _scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (_positiveDrag)
        {
            newPos.y = endItem.position.y - _scrollContent.childHeight * 1.5f + _scrollContent.itemSpacing;
        }
        else
        {
            newPos.y = endItem.position.y + _scrollContent.childHeight * 1.5f - _scrollContent.itemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }

    private bool ReachedThreshold(Transform item)
    {
        float posYThreshold = transform.position.y + _scrollContent.height * 0.5f + _outOfBoundsThreshold;
        float negYThreshold = transform.position.y - _scrollContent.height * 0.5f - _outOfBoundsThreshold;
        return _positiveDrag ? item.position.y - _scrollContent.childWidth * 0.5f > posYThreshold :
            item.position.y + _scrollContent.childWidth * 0.5f < negYThreshold;
    }
}
