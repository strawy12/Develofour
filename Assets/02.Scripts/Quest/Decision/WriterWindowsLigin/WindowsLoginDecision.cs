using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WindowsLoginDecision : Decision
{
    private bool isLoginWindows;

    [SerializeField]
    private EChapterDataType chapterDataType;

    public override void Init()
    {
        EventManager.StartListening(EWindowEvent.WindowsSuccessLogin, WindowsLogin);
    }

    private void WindowsLogin(object[] empty)
    {
        if (DataManager.Inst.CurrentPlayer.chapterDatas[(int)chapterDataType].isLoginWindows)
        {
            isLoginWindows = true;
        }

        OnChangedValue?.Invoke();
        EventManager.StopListening(EWindowEvent.WindowsSuccessLogin, WindowsLogin);
    }

    public override bool CheckDecision()
    {
        return isLoginWindows;
    }


}
