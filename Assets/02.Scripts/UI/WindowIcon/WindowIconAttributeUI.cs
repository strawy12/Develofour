using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WindowIconAttributeUI : MonoUI
{
    static public Action<Vector2, FileSO> OnCreateMenu;

    [SerializeField]
    private Button openPropertyBtn;

    [SerializeField]
    private PropertyUI propertyUI;

    public RectTransform rectTransform;

    private FileSO windowPropertyData;
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

        openPropertyBtn.onClick?.AddListener(CreateProperty);
    }

    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;

        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            CloseMenu();
        }
    }

    public void CreateMenu(Vector2 mousePos, FileSO fileData)
    {
        Vector2 offset = Vector2.zero;

        if (Mathf.Abs(Constant.MAX_CANVAS_POS.x - mousePos.x) < rectTransform.rect.width)
        {
            offset.x += rectTransform.rect.width - Mathf.Abs((Constant.MAX_CANVAS_POS.x - mousePos.x));
        }

        if (Mathf.Abs(-Constant.MAX_CANVAS_POS.y - mousePos.y) < rectTransform.rect.height)
        {
            offset.y += rectTransform.rect.height - Mathf.Abs((Constant.MAX_CANVAS_POS.y - mousePos.y));
        }

        mousePos += offset;

        rectTransform.anchoredPosition = mousePos;

        SetActive(true);
        windowPropertyData = fileData;
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
        Debug.Log("click");
        Debug.Log(windowPropertyData.name);
        propertyUI.CreatePropertyUI(windowPropertyData);
        CloseMenu();
    }
}
