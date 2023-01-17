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

    private bool _scrolling;

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

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _lastDragPosition = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _positiveDrag = eventData.position.y > _lastDragPosition.y;

        _lastDragPosition = eventData.position;
    }

    void IScrollHandler.OnScroll(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _positiveDrag = eventData.scrollDelta.y > 0;
        _scrolling = true;
    }

    public void StartIEOnViewScroll()
    {
        StartCoroutine(IEOnViewScroll());
    }

    IEnumerator IEOnViewScroll()
    {
        yield return new WaitForSeconds(0.1f);
        if (_scrolling)
        {
            OnViewScroll();
            _scrolling = false;
        }
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
            newPos.y = endItem.position.y - _scrollContent.childHeight * .7f + _scrollContent.itemSpacing;
        }
        else
        {
            newPos.y = endItem.position.y + _scrollContent.childHeight * .7f - _scrollContent.itemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }
    private bool ReachedThreshold(Transform item)
    {
        float posYThreshold = transform.position.y + _scrollContent.height * -.035f + _outOfBoundsThreshold;
        float negYThreshold = transform.position.y - _scrollContent.height * .55f - _outOfBoundsThreshold;
        return _positiveDrag ? item.position.y - _scrollContent.childHeight * -.035f > posYThreshold :
            item.position.y + _scrollContent.childHeight * .55f < negYThreshold;
    }
}