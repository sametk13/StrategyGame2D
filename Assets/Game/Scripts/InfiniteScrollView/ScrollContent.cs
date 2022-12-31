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
        ProductionMenuHandler.OnProductionChange += InitializeContentVertical;
    }

    private void OnDisable()
    {
        ProductionMenuHandler.OnProductionChange -= InitializeContentVertical;
    }

    private void Start()
    {

        InitializeContentVertical();
    }

    private void InitializeContentVertical()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform.childCount == 0) return;

        _rtChildren = new RectTransform[_rectTransform.childCount];

        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            _rtChildren[i] = _rectTransform.GetChild(i) as RectTransform;
        }

        _height = _rectTransform.rect.height - (2 * verticalMargin);

        _childWidth = _rtChildren[0].rect.width;
        _childHeight = _rtChildren[0].rect.height;

        float originY = 0 - (height * 0.5f);
        float posOffset = childHeight * 0.5f;
        for (int i = 0; i < _rtChildren.Length; i++)
        {
            Vector2 childPos = _rtChildren[i].localPosition;
            childPos.y = originY + posOffset + i * (childHeight + itemSpacing);
            _rtChildren[i].localPosition = childPos;
        }
    }
}
