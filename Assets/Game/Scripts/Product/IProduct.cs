using SKUtils.Feedbacks;
using System;
using UnityEngine;

public interface IProduct : ISelectable
{

    public SpriteRenderer spriteRenderer { get; set; }

    public PunchScaleFeedBack punchScaleFeedBack { get; set; }


}
