using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Product : MonoBehaviour,ISelectable
{
    public Action OnSelected;
    public Action OnUnSelected;

    public SpriteRenderer spriteRenderer { get; set; }

    public void OnEnable()
    {
        OnSelected += Selected;
        OnUnSelected += UnSelected;
    }
    public void OnDisable()
    {
        OnSelected -= Selected;
        OnUnSelected -= UnSelected;
    }

    public abstract void Selected();

    public abstract void UnSelected();  
}
