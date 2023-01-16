using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TargetWindowPanels : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    private bool isEnter;
    private bool isShow;

    public bool IsShow => isShow;

    [SerializeField]
    protected TargetWindowPanel targetWindowPanelTemp;

    private const float HIDE_DELAY_TIME = 1.25f;

    private Coroutine hideDelayCoroutine = null;

    public Func<bool> OnUnSelectIgnoreFlag;

    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }



    public void CheckClose(object[] hits)
    {
        if (gameObject.activeSelf == false) return;
        if (OnUnSelectIgnoreFlag != null && OnUnSelectIgnoreFlag.Invoke()) return;

        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            isEnter = false;
            Hide();
        }

    }

    public void Show()
    {
        Debug.Log("11");
        SetActive(true);
        isShow = true;
    }
    public void Hide()
    {
        Debug.Log("22");
        SetActive(false);
        isShow = false;
    }

    public TargetWindowPanel AddTargetPanel(Window targetWindow)
    {
        if (targetWindow == null) { return null; }

        TargetWindowPanel panel = GetTargetPanel();
        panel.Init(targetWindow);

        return panel;
    }

    private TargetWindowPanel GetTargetPanel()
    {
        // Ǯ���� �� ���� �־� �Լ��� ���� ��

        TargetWindowPanel panel = null;

        if (panel == null)
        {
            panel = Instantiate(targetWindowPanelTemp, targetWindowPanelTemp.transform.parent);
        }

        panel.gameObject.SetActive(true);
        return panel;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;

        if (hideDelayCoroutine != null)
        {
            StopCoroutine(hideDelayCoroutine);
        }

        hideDelayCoroutine = StartCoroutine(HideDelay());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;

        if (hideDelayCoroutine != null)
        {
            StopCoroutine(hideDelayCoroutine);
            hideDelayCoroutine = null;
        }
    }

    private IEnumerator HideDelay()
    {
        yield return new WaitForSeconds(HIDE_DELAY_TIME);

        Hide();
        hideDelayCoroutine = null;
    }
}
