using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiscordMessagePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Message")]
    [SerializeField]
    private DiscordMessageText messageText;
    [SerializeField]
    private TMP_Text userNameText;
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private DiscordMessageImagePanel messageImagePanel;

    [Header("PanelSizeSetting")]
    [SerializeField]
    private float spacing;

    private Image backgroundImage;

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
        backgroundImage ??= GetComponent<Image>();
        rectTransform ??= GetComponent<RectTransform>();

        messageText.SettingMessage("");
        userNameText.text = "";

        profileImage.sprite = null;
    }

    public void SettingChatData(DiscordChatData data, DiscordProfileDataSO profileData, bool showProfile)
    {
        currentChatData = data;
        currentProfileData = profileData;
        messageText.SettingMessage(data.message);
        if (data.msgSpritePrefab != null)
        {
            messageImagePanel.SettingImage(data.msgSpritePrefab.GetComponent<Image>().sprite);
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
        else
        {
            profileImage.gameObject.SetActive(false);
            userNameText.gameObject.SetActive(false);
        }
        AutoSettingMessagePanelSize();
    }

    public void AutoSettingMessagePanelSize(bool isDifferentUserName = false) 
    {
        float newVertical = 0f;

        if (messageText.gameObject.activeSelf)
        {
            newVertical += messageText.MessageRect.sizeDelta.y;
        }
        if (userNameText.gameObject.activeSelf)
        {
            newVertical += userNameText.rectTransform.sizeDelta.y;
        }
        if (messageImagePanel.gameObject.activeSelf)
        {
            newVertical += messageImagePanel.SizeDelta.y;
        }
        if(isDifferentUserName)
        {
            newVertical += 15;
        }
        newVertical += spacing;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newVertical);
    }

    public void Release()
    {
        messageText.SettingMessage("");
        messageImagePanel.Release();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backgroundImage.enabled = true;

        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if(currentChatData.infoData != null)
        {
            CursorChangeSystem.ECursorState state = Define.ChangeInfoCursor(currentChatData.needInformaitonList, currentChatData.infoData.category, currentChatData.infoData.key);
            if(state == CursorChangeSystem.ECursorState.FindInfo)
            {
                messageText.SetColor(Color.yellow);
            }
            else if (state == CursorChangeSystem.ECursorState.FoundInfo)
            {
                messageText.SetColor(Color.red);
            }
        }
            
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.enabled = false;
        messageText.SetColor(Color.white);
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[1] { CursorChangeSystem.ECursorState.Default });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentChatData.msgSpritePrefab != null)
            {
                EventManager.TriggerEvent(EDiscordEvent.ShowImagePanel, new object[1] { currentChatData.msgSpritePrefab }); ;
            }
            if (currentChatData.infoData != null)
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { currentChatData.infoData.category, currentChatData.infoData.key, null });
            }
        }

        OnPointerEnter(eventData);
    }
}
