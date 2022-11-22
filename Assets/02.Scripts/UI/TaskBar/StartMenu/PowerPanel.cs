using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class PowerPanel : MonoUI
{
    [SerializeField]
    private Button agreeBtn;

    [SerializeField]
    private Button cancelBtn;

    private RectTransform rectTransform;

    private bool isOpen = false;
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
        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);
    }

    private void CheckClose(object hits)
    {
        if (isOpen == false) { return; }
        if (Define.ExistInHits(gameObject, hits) == false)
        {
            Close();
        }
    }

    public void Show()
    {
        EventManager.TriggerEvent(EEvent.ActivePowerPanel, new object[1] { true });
        SetActive(true);
        isOpen = true;
    }

    private void Close()
    {
        StartCoroutine(CloseEventDelay());
        SetActive(false);
        isOpen = false;
    }

    private IEnumerator CloseEventDelay()
    {
        yield return new WaitForEndOfFrame();
        EventManager.TriggerEvent(EEvent.ActivePowerPanel, new object[1] { true });
    }

}
