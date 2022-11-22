using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindowIconAttributeUI : MonoUI
{
    static public Action<Vector2, WindowIconDataSO> OnCreateMenu;

    [SerializeField]
    private PropertyUI propertyUI;

    public RectTransform rectTransform;

    private WindowIconDataSO windowPropertyData;
    private bool isOpen = false;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();

        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        OnCreateMenu += CreateMenu;
    }

    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;

        if (Define.ExistInHits(gameObject, hits) == false)
        {
            CloseMenu();
        }
    }

    public void CreateMenu(Vector2 mousePos, WindowIconDataSO windowIconData)
    {
        Vector2 offset = Vector2.zero;

        if (Mathf.Abs(Constant.MAXCANVASPOS.x - mousePos.x) < rectTransform.rect.width)
        {
            offset.x += rectTransform.rect.width - Mathf.Abs((Constant.MAXCANVASPOS.x - mousePos.x));
        }

        if (Mathf.Abs(-Constant.MAXCANVASPOS.y - mousePos.y) < rectTransform.rect.height)
        {
            offset.y += rectTransform.rect.height - Mathf.Abs((Constant.MAXCANVASPOS.y - mousePos.y));
        }

        mousePos += offset;

        rectTransform.anchoredPosition = mousePos;

        SetActive(true);
        windowPropertyData = windowIconData;
        isOpen = true;
    }

    public void CloseMenu()
    {
        SetActive(false);
        windowPropertyData = null;
        isOpen = false;
    }

    public void CreateProperty()
    {
        propertyUI.CreatePropertyUI(windowPropertyData);
        CloseMenu();
    }
}
