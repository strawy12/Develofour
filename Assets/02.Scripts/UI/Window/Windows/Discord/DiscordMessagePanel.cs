using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscordMessagePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private TMP_Text userNameText;
    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private Image messageImage;

    private DiscordProfileDataSO currentProfileData;

    public void Init()
    {
        currentProfileData = null;

        messageText.text = null;
        userNameText.text = null;
        timeText.text = null;

        profileImage.sprite = null;
        messageImage.sprite = null;
    }

    public void SettingChatData(DiscordChatData data, DiscordProfileDataSO profileData)
    {
        currentProfileData = profileData;
        
        messageText.text = data.message;
        userNameText.text = profileData.userName;
        timeText.text = data.sendTimeText;

        profileImage.sprite = profileData.userSprite;
        
        if (data.msgSprite != null)
        {
            messageImage.sprite = data.msgSprite;
        }
        else
        {
            messageImage.gameObject.SetActive(false);
        }
    }
}
