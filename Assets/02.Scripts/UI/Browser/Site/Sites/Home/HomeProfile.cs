using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeProfile : MonoBehaviour, IPointerClickHandler
{
    public HomeProfileLoginPanel LoginPanel;
    public void Init()
    {
        LoginPanel.Init();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LoginPanel.gameObject.SetActive(true);
    }
}
