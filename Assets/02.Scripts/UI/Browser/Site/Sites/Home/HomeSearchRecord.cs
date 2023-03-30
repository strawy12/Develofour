using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class HomeSearchRecord : MonoBehaviour
{
    public Action OnCloseRecord;
    private bool isOpen;

    public void OpenPanel()
    {
        isOpen = true;
        gameObject.SetActive(true);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
        isOpen = false;
        OnCloseRecord?.Invoke();
    }
}
