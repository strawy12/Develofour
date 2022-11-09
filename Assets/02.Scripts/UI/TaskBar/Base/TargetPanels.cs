using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.EditorCoroutines.Editor;
using DG.Tweening;

public class TargetPanels : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<TaskIcon> OnOpened;
    public static Action OnClosed;

    [SerializeField]
    private float delayTime = 2f;

    private Transform originParent;

    private TaskIcon targetTaskIcon;
    private List<TargetPanel> targetPanelList;

    private RectTransform rectTransform;

    private bool isShow;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originParent = transform.parent;

        targetPanelList = new List<TargetPanel>();
        isShow = false;

        OnOpened += Open;
        OnClosed += Close;
    }

    private void Open(TaskIcon icon = null)
    {
        if (icon != null)
        {
            targetPanelList = icon.TargetPanelList;
            
            foreach (var targetPanel in targetPanelList)
            {
                targetPanel.transform.SetParent(transform);
                targetPanel.rectTransform.anchoredPosition = Vector3.zero;
            }
        }

        StopAllCoroutines();
        if (isShow == false)
        {
            StartCoroutine(DelayCoroutine(Show));
        }
        else
        {
            Show();
        }
    }

    private void Close()
    {
        if (isShow == false) return;
        StartCoroutine(DelayCoroutine(Hide));
    }

    private void Show()
    {
        if (isShow) return;
        isShow = true;
        SetActive(true);
    }

    private void Hide()
    {
        if (!isShow) return;
        isShow = false;
        SetActive(false);
    }

    private IEnumerator DelayCoroutine(Action callBack)
    {
        yield return new WaitForSeconds(delayTime);

        callBack?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Open();
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        Close();
    }
}
