using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{ 
    public static readonly Vector2 MAXWINSIZE = new Vector2(1920, 1080);
    public static readonly Vector2 MAXCANVASPOS = new Vector2(960, 540);
    public static readonly Vector2 WINDOWICONSIZE = new Vector2(100, 100);
    public static readonly Vector2 WINDOWDEFAULTPOS = new Vector2(-890, 460);

    public static readonly Vector2 NOTICE_POS = new Vector2(-17.5f, -400f);
    
    public static readonly float NOTICE_DELAYTIME = 5f;
    public static readonly float NOTICE_DURATION = 0.5f;

    public static readonly EWindowType BROWSER_KEY = EWindowType.Browser;

    public const float LOADING_DELAY = 0.75f;
}
