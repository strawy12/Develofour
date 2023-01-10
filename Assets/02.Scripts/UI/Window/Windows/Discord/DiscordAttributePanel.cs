using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DiscordAttributePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DiscordProfileDataSO data;
    public GameObject bluePanel;
    public void OnPointerEnter(PointerEventData eventData)
    {
        bluePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bluePanel.gameObject.SetActive(false);
    }
}
