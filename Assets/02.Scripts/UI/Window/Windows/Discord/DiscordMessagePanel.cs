using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscordMessagePanel : MonoBehaviour
{
    [Header("Message")]
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private TMP_Text userNameText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private DiscordMessageImagePanel messageImagePanel;

    [Header("PanelSizeSetting")]
    [SerializeField]
    private float spacing;

    private DiscordProfileDataSO currentProfileData;
    private DiscordChatData currentChatData;
    public DiscordChatData ChatData
    {
        get
        {
            return currentChatData;
        }
    }
    public string UserName
    {
        get
        {
            return currentProfileData.userName;
        }
    }
    private RectTransform rectTransform;
    public void Init()
    {
        rectTransform ??= GetComponent<RectTransform>();

        messageText.text = "";
        userNameText.text = "";
        //timeText.text = null;

        profileImage.sprite = null;
    }

    public void SettingChatData(DiscordChatData data, DiscordProfileDataSO profileData, bool showProfile)
    {
        currentChatData = data;
        currentProfileData = profileData;
        messageText.text = data.message;
        //timeText.text = data.sendTimeText;
        //timeText.gameObject.SetActive(true);
        if (data.msgSprite != null)
        {
            messageImagePanel.SettingImage(data.msgSprite);
            messageImagePanel.gameObject.SetActive(true);
        }
        else
        {
            messageImagePanel.gameObject.SetActive(false);
        }

        if (showProfile)
        {
            profileImage.sprite = profileData.userSprite;
            userNameText.text = profileData.userName;

            profileImage.gameObject.SetActive(true);
            userNameText.gameObject.SetActive(true);

        }
    }

    public void AutoSettingMessagePanelSize()
    {
        float newVertical = 0f;

        if (messageText.gameObject.activeSelf)
        {
            newVertical += messageText.rectTransform.sizeDelta.y + spacing;
        }
        if (userNameText.gameObject.activeSelf)
        {
            newVertical += userNameText.rectTransform.sizeDelta.y + spacing;
        }
        if (messageImagePanel.gameObject.activeSelf)
        {
            newVertical += messageImagePanel.SizeDelta.y + spacing;
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newVertical);
    }

    public void Release()
    {
        messageText.text = "";
        messageImagePanel = null;
        messageImagePanel.Release();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
    }
}
