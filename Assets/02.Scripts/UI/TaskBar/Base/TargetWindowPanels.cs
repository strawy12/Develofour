using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TargetWindowPanels : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    private bool isEnter;
    public bool IsEnter { get { return isEnter; } }
    private Image targetWindowPanelUI;
    public RectTransform TargetTransform 
    {
        get 
        { 
            if(targetWindowPanelUI == null) targetWindowPanelUI = GetComponent<Image>();
            return targetWindowPanelUI?.rectTransform;
        } 
    }
    private void Awake()
    {
        targetWindowPanelUI = GetComponent<Image>();
    }
    public void Init()
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);

    }

    public void CheckClose(object hits)
    {
        if (gameObject.activeSelf == false) return;
        if(Define.ExistInHits(gameObject, hits) == false)
        {
            isEnter = false;
            CloseTargetWindowPanelUI();
        }
    }
    public void OpenTargetWindowPanelUI()
    {
        targetWindowPanelUI.gameObject.SetActive(true);
    }
    public void CloseTargetWindowPanelUI()
    {
        targetWindowPanelUI.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }
}
