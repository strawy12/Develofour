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

    [SerializeField]
    private Image windowIcon;

    public Action OnCloseWindow;
    public Action OnOpenWindow;

    private FileSO file;

    public void Init(FileSO fileData)
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        file = fileData;
        windowIcon.sprite = file.iconSprite;

        switch (file.taskBarData.openType)
        {
            case ETaskBarOpenType.Open:
                openButton.onClick.AddListener(WindowOpen);
                break;
            case ETaskBarOpenType.Clone:
                openButton.onClick.AddListener(WindowClone);
                break;
            case ETaskBarOpenType.CreateOrigin:
                openButton.onClick.AddListener(WindowCreateOrigin);
                break;

        }

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
        openButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void WindowOpen()
    {
        OnOpenWindow?.Invoke();
    }

    public void WindowClose()
    {
        Debug.Log("WindowClose");
        OnCloseWindow?.Invoke();
    }

    public void WindowClone()
    {
        WindowManager.Inst.CreateWindow(file.windowType, file);
    }
    public void WindowCreateOrigin()
    {
        WindowManager.Inst.CreateWindow(file.windowType);
    }

}