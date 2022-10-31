using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerPanel : MonoUI
{
    [SerializeField]
    private Button agreeBtn;

    [SerializeField]
    private Button cancelBtn;

    private RectTransform rectTransform;


    private void Awake()
    {
        Init();
        Bind();
    }

    private void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Init()
    {
        agreeBtn.onClick.AddListener(Define.GameQuit);
        cancelBtn.onClick.AddListener(Close);
    }

    public void Show()
    {
        EventManager.TriggerEvent(EEvent.ActivePowerPanel, true);
        SetActive(true);
    }

    private void Close()
    {
        EventManager.TriggerEvent(EEvent.ActivePowerPanel, false);
        SetActive(false);
    }


}
