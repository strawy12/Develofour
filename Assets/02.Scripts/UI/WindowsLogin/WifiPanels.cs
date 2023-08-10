using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class WifiPanels : MonoUI
{
    [SerializeField]
    private HighlightBtn wifiBtn;
    private RectTransform rectTransform;

    private float expendSizeY = 0f;

    private bool isShow =false;
    private bool isComplete = true;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        expendSizeY = rectTransform.sizeDelta.y;
        wifiBtn.OnClick += Show;

        EventManager.StartListening(ECoreEvent.LeftButtonClick, TryHide);
    }

    public void Show()
    {
        if (isShow) return;
        if (isComplete == false) return;
        isShow = true; 
        SetActive(true);
        isComplete = false;
        rectTransform.sizeDelta *= Vector2.right;
        DOTween.To(
            () => rectTransform.sizeDelta.y,
            (value) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value),
            expendSizeY,
            0.2f
        ).SetEase(Ease.InCubic).Complete(isComplete = true);
        
    }

    private void TryHide(object[] hits)
    {
        if (isShow == false) return;

        if (Define.ExistInHits(gameObject, hits[0]) == false && Define.ExistInHits(wifiBtn.gameObject, hits[0]) == false)
        {
            Hide();
        }
    }

    public void Hide()
    {
        if (isShow == false) return;
        isShow = false;

        DOTween.Kill(rectTransform);
        rectTransform.sizeDelta *= Vector2.right;

        SetActive(false);
    }

}
