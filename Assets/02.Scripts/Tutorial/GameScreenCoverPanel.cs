using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenCoverPanel : MonoBehaviour
{
    private void Awake()
    {
        EventManager.StartListening(ECoreEvent.CoverPanelSetting, ScreenSetting);
    }


    private void ScreenSetting(object[] ps)
    {
        if(ps[0] ==null ||!(ps[0] is bool))
        {
            return;
        }

        bool value = (bool)ps[0];

        gameObject.SetActive(value);
    }
}
