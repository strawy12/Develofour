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
    public RectTransform TargetTransform { get { return targetWindowPanelUI.rectTransform; } }
    private void Awake()
    {
        targetWindowPanelUI = GetComponent<Image>();
    }
    public void Init()
    {
        
    }

    public void OpenTargetWindowPanelUI()
    {
        if (targetWindowPanelUI.gameObject.activeSelf) return;
        targetWindowPanelUI.gameObject.SetActive(true);
    }
    public void CloseTargetWindowPanelUI()
    {
        Debug.Log("false");
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
