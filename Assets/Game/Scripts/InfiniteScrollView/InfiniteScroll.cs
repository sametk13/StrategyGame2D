using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{

    [SerializeField]
    private ScrollContent scrollContent;

    [SerializeField]
    private float outOfBoundsThreshold;
    private ScrollRect scrollRect;

    private Vector2 lastDragPosition;

    private bool positiveDrag;


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
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        positiveDrag = eventData.position.y > lastDragPosition.y;

        lastDragPosition = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        positiveDrag = eventData.scrollDelta.y > 0;
    }

    public void StartIEOnViewScroll()
    {
        StartCoroutine(IEOnViewScroll());
    }

    IEnumerator IEOnViewScroll()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("IEOnViewScroll");
        OnViewScroll();
    }

    public void OnViewScroll()
    {
        HandleVerticalScroll();
    }

    private void HandleVerticalScroll()
    {
        if (scrollRect.content.childCount == 0) return;
        int currItemIndex = positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currItem = scrollRect.content.GetChild(currItemIndex);

        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform endItem = scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (positiveDrag)
        {
            newPos.y = endItem.position.y - scrollContent.ChildHeight * 1.5f + scrollContent.ItemSpacing;
        }
        else
        {
            newPos.y = endItem.position.y + scrollContent.ChildHeight * 1.5f - scrollContent.ItemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }

    private bool ReachedThreshold(Transform item)
    {
        float posYThreshold = transform.position.y + scrollContent.Height * 0.5f + outOfBoundsThreshold;
        float negYThreshold = transform.position.y - scrollContent.Height * 0.5f - outOfBoundsThreshold;
        return positiveDrag ? item.position.y - scrollContent.ChildWidth * 0.5f > posYThreshold :
            item.position.y + scrollContent.ChildWidth * 0.5f < negYThreshold;
    }
}
