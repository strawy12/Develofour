using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenCoverPanel : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(ECoreEvent.CoverPanelSetting, ScreenSetting);

        gameObject.SetActive(false);
    }


    private void ScreenSetting(object[] ps)
    {
        if(ps[0] ==null ||!(ps[0] is bool))
        {
            return;
        }

        bool value = (bool)ps[0];

        Debug.Log(112);
        gameObject.SetActive(value);
    }
}
