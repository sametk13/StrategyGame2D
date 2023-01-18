using UnityEngine;

public class ScrollContent : MonoBehaviour
{
    public float itemSpacing { get { return _itemSpacing; } }
    public float verticalMargin { get { return _verticalMargin; } }
    public float height { get { return _height; } }
    public float childWidth { get { return _childWidth; } }
    public float childHeight { get { return _childHeight; } }


    private RectTransform _rectTransform;
    private RectTransform[] _rtChildren;
    private float _height;
    private float _childWidth, _childHeight;

    [SerializeField]
    private float _itemSpacing;
    [SerializeField]
    private float _verticalMargin;

    private void OnEnable()
    {
        // Subscribe to the OnProductionChange event
        ProductionMenuManager.OnProductionChange += InitializeContentVertical;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnProductionChange event
        ProductionMenuManager.OnProductionChange -= InitializeContentVertical;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // function to initialize the content of the scroll view
    private void InitializeContentVertical()
    {
        // reset the position of the scroll view
        _rectTransform.localPosition = Vector2.zero;
        if (_rectTransform.childCount == 0) return;

        _rtChildren = new RectTransform[_rectTransform.childCount];

        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            _rtChildren[i] = _rectTransform.GetChild(i) as RectTransform;
        }


        // calculate the height of the scroll view and the width and height of each child
        _height = _rectTransform.rect.height - (2 * _verticalMargin);
        _childWidth = _rtChildren[0].rect.width;
        _childHeight = _rtChildren[0].rect.height;


        // calculate the y position of each child
        float originY = 0 - (_height * 0.5f);
        float posOffset = _childHeight * 0.5f;
        for (int i = 0; i < _rtChildren.Length; i++)
        {
            Vector2 childPos = _rtChildren[i].localPosition;
            childPos.y = originY + posOffset + i * (_childHeight * _itemSpacing);
            childPos.x = 0;
            _rtChildren[i].localPosition = childPos;
        }
    }
}