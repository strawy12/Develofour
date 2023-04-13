using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneCallUI : MonoUI
{
    public void Open()
    {
        SetActive(true);

        Init();
    }

    private void Init()
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void CheckClose(object[] hits)
    {
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            SetActive(false);
        }

        EventManager.StopAllListening(ECoreEvent.LeftButtonClick);
    }
}
