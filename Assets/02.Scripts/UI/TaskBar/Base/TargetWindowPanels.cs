using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetWindowPanels : MonoBehaviour
{
    //void OnEnable()
    //{
    //    EventManager.StartListening(EEvent.LeftButtonClick, (x) => CloseTargetWindowPanelUI());
    //}

    //void OnDisable()
    //{
    //    EventManager.StopListening(EEvent.LeftButtonClick, (x) => CloseTargetWindowPanelUI());
    //}

    private Image targetWindowPanelUI;
    public RectTransform TargetTransform { get { return targetWindowPanelUI.rectTransform; } }
    private void Awake()
    {
        targetWindowPanelUI = GetComponent<Image>();
    }
    public void Init()
    {
        //EventManager.StartListening(EEvent.LeftButtonClick, (x) => CloseTargetWindowPanelUI());
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
}
