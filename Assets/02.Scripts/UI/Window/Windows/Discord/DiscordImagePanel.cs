using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiscordImagePanel : MonoBehaviour, IPointerClickHandler
{
    public Image profileImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.gameObject.SetActive(false);
        }
    }
}
