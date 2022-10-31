using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TaskIconAttribute : MonoBehaviour
{

    [SerializeField]
    private Button openButton;

    [SerializeField]
    private Button closeButton;

    public Action OnClose;
    public Action OnOpen;

    public void Init()
    {
        //TODO 이거 안되는거 고치기
        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);
        openButton.onClick.AddListener(AttributeOpen);
        closeButton.onClick.AddListener(AttributeClose);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void CheckClose(object hits)
    {
        if (gameObject.activeSelf == false) return;
        if (Define.ExistInHits(gameObject, hits) == false)
        {
            Close();
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void AttributeClose()
    {
        OnClose?.Invoke();
        Close();
    }

    public void AttributeOpen()
    {
        OnOpen?.Invoke();
        Close();
    }

    
}
