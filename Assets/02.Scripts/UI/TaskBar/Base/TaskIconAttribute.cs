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

    public Action OnCloseWindow;
    public Action OnOpenWindow;

    public void Init()
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        openButton.onClick.AddListener(WindowOpen);
        closeButton.onClick.AddListener(WindowClose);
    }
    public void CheckClose(object[] hits)
    {
        if (gameObject.activeSelf == false) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void WindowOpen()
    {
        OnOpenWindow?.Invoke();
    }

    public void WindowClose()
    {
        OnCloseWindow?.Invoke();
    }



}