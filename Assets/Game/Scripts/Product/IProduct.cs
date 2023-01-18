using SKUtils.Feedbacks;
using UnityEngine;

public interface IProduct : ISelectable
{
    public SpriteRenderer spriteRenderer { get; set; }

    public PunchScaleFeedBack punchScaleFeedBack { get; set; }
}
