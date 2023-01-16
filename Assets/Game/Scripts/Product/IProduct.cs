using System;

public interface IProduct : ISelectable
{
    public Action OnSelect { get; set; }
    public Action OnUnSelect { get; set; }
    public  bool isSelected { get; set; }
}
