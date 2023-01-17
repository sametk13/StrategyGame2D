using System;

public interface ISelectable
{
    public Action OnSelect { get; set; }
    public Action OnUnSelect { get; set; }
    public bool isSelected { get; set; }

    public void Selected();
    public void UnSelected();


}
