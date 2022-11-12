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

    public Action OnCloseTaskIcon;
    public Action OnOpenWindow;

    public void Init()
    {
        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);
        openButton.onClick.AddListener(() => OnCloseTaskIcon?.Invoke());
        closeButton.onClick.AddListener(() => OnOpenWindow?.Invoke());
    }

    private void Open()
    {
        gameObject.SetActive(true);
    }
    public void CheckClose(object hits)
    {
        if (gameObject.activeSelf == false) return;
        if (Define.ExistInHits(gameObject, hits) == false)
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



}