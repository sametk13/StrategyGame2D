using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{
    [SerializeField] private ScrollContent _scrollContent;
    [SerializeField] private float _outOfBoundsThresholdUp;
    [SerializeField] private float _outOfBoundsThresholdDown;

    private ScrollRect _scrollRect;
    private Vector2 _lastDragPosition;
    private bool _positiveDrag;
    private bool _scrolling;

    private void OnEnable()
    {
        // Subscribe to the OnProductionChange event
        ProductionMenuManager.OnProductionChange += OnViewScroll;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnProductionChange event
        ProductionMenuManager.OnProductionChange -= OnViewScroll;
    }

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.vertical = true;

        //set the movement type to unrestricted
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
    }


    //Event called when dragging begins
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _lastDragPosition = eventData.position;
    }


    //Event called when dragging happens
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _positiveDrag = eventData.position.y > _lastDragPosition.y;

        _lastDragPosition = eventData.position;
    }
    //Event called when scrolling happens
    void IScrollHandler.OnScroll(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        _positiveDrag = eventData.scrollDelta.y > 0;
        _scrolling = true;
    }

    private void OnViewScroll()
    {
        HandleVerticalScroll();
    }

    //Method to handle vertical scrolling
    private void HandleVerticalScroll()
    {
        if (_scrollRect.content.childCount == 0) return;

        int currItemIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;  //get the current item index

        var currItem = _scrollRect.content.GetChild(currItemIndex); //get the current item

        if (!ReachedThreshold(currItem)) //if the threshold is not reached
        {
            return;
        }

        int endItemIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
        Transform endItem = _scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (_positiveDrag)
        {
            newPos.y = endItem.position.y - _scrollContent.childHeight * _scrollContent.itemSpacing;
        }
        else
        {
            newPos.y = endItem.position.y + _scrollContent.childHeight * _scrollContent.itemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }
    private bool ReachedThreshold(Transform item)
    {
        float posYThreshold = transform.position.y + _scrollContent.height * -.035f + _outOfBoundsThresholdUp;
        float negYThreshold = transform.position.y - _scrollContent.height * .55f - _outOfBoundsThresholdDown;
        return _positiveDrag ? item.position.y - _scrollContent.childHeight * -.035f > posYThreshold :
            item.position.y + _scrollContent.childHeight * .55f < negYThreshold;
    }
}