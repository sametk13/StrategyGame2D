using System;
using UnityEngine;

public abstract class Product : MonoBehaviour,ISelectable
{
    public Action OnSelect;
    public Action OnUnSelect;

    public SpriteRenderer spriteRenderer { get; set; }

    public void OnEnable()
    {
        OnSelect += Selected;
        OnUnSelect += UnSelected;
    }
    public void OnDisable()
    {
        OnSelect -= Selected;
        OnUnSelect -= UnSelected;
    }

    public abstract void Selected();

    public abstract void UnSelected();  
}
