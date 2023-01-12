using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DiscordProfilePanel : MonoBehaviour, IPointerClickHandler
{
    //AttributePanel -> Profile
    public Image profileImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI memoText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ReSetting(DiscordProfileDataSO data)
    {
        profileImage.sprite = data.userSprite;
        nameText.text = data.userName;
        memoText.text = data.infoMsg;
        this.gameObject.SetActive(true);
    }
}
