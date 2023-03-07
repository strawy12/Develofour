using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundPanelButton : MonoBehaviour, IPointerDownHandler
{
    public SoundPanel soundPanel;

    public void OnPointerDown(PointerEventData eventData)
    {

        if(soundPanel.GetComponent<CanvasGroup>().interactable == true)
        {
            soundPanel.Close();
        }
        else
        {
            soundPanel.OpenPanel();
        }    
    }
}
