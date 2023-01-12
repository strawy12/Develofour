using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DiscordAttributePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    public DiscordProfileDataSO data;
    
    public GameObject bluePanel;

    public DiscordProfilePanel profilePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        //프로필 열어줌
        if(eventData.button == PointerEventData.InputButton.Left)
        { 
            if(data == null)
            {
                Debug.Log("Error : data is null. Find 'DiscordFriendList.cs RightClickLine Function'");
            }
            else
            {
                profilePanel.ReSetting(data);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bluePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bluePanel.gameObject.SetActive(false);
    }
}
