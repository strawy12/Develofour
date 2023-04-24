using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TargetWindowPanels : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
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
            Hide();
        }

    }

    public void Show()
    {
        SetActive(true);
        isShow = true;
    }
    public void Hide()
    {
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
        // 풀링을 할 수도 있어 함수를 따로 뺌

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

        if (hideDelayCoroutine != null)
        {
            StopCoroutine(hideDelayCoroutine);
        }

        hideDelayCoroutine = StartCoroutine(HideDelay());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
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
