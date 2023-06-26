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
    [SerializeField]
    private TMP_Text userTimeText;
    [SerializeField]
    private DiscordTimeText timeText;
    [Header("PanelSizeSetting")]
    [SerializeField]
    private float spacing;

    private Image backgroundImage;

    private DiscordProfileDataSO currentProfileData;
    private DiscordChatData currentChatData;

    private bool isProfileShow;

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
        timeText.Init();
        profileImage.sprite = null;
    }

    public void SettingChatData(DiscordChatData data, DiscordProfileDataSO profileData, bool showProfile, DiscordSendTime sendTime, bool isFirst)
    {
        currentChatData = data;
        currentProfileData = profileData;
        isProfileShow = showProfile;
        messageText.SettingMessage(data.message);
        timeText.SettingText(sendTime);

        if (data.msgSpritePrefab != null)
        {
            messageImagePanel.SettingImage(data.msgSpritePrefab.GetComponent<Image>().sprite);
            messageImagePanel.gameObject.SetActive(true);
        }
        else
        {
            messageImagePanel.gameObject.SetActive(false);
        }

        if (showProfile || isFirst)
        {
            profileImage.sprite = profileData.userSprite;
            userNameText.text = profileData.userName;

            profileImage.gameObject.SetActive(true);
            userNameText.gameObject.SetActive(true);
            userTimeText.gameObject.SetActive(true);
            userNameText.transform.parent.gameObject.SetActive(true);
            userTimeText.text = timeText.TimeText;
        }
        else
        {
            profileImage.gameObject.SetActive(false);
            userNameText.gameObject.SetActive(false);
            userNameText.transform.parent.gameObject.SetActive(false);
            userTimeText.gameObject.SetActive(false);
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
        if (isDifferentUserName)
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
        if (!isProfileShow)
        {
            timeText.gameObject.SetActive(true);
        }
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (currentChatData.infoIDs.Count != 0 || currentChatData.monologID != 0)
        {
            CursorChangeSystem.ECursorState state = Define.ChangeInfoCursor(currentChatData.needInformaitonList, currentChatData.infoIDs);
            if(currentChatData.infoIDs.Count == 0)
            {
                state = CursorChangeSystem.ECursorState.FindInfo;
            }
                if (state == CursorChangeSystem.ECursorState.FindInfo || state == CursorChangeSystem.ECursorState.NeedInfo)
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
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[1] { CursorChangeSystem.ECursorState.Default });
        backgroundImage.enabled = false;
        timeText.gameObject.SetActive(false);
        messageText.SetColor(Color.white);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentChatData.msgSpritePrefab != null)
            {
                EventManager.TriggerEvent(EDiscordEvent.ShowImagePanel, new object[1] { currentChatData.msgSpritePrefab }); ;
            }
            if (currentChatData.needInformaitonList.Count != 0)
            {
                foreach (var needInfo in currentChatData.needInformaitonList)
                {
                    if (!DataManager.Inst.IsProfilerInfoData(needInfo.needInfoID))
                    {
                        MonologSystem.OnStartMonolog?.Invoke(needInfo.monologID, 0f, false);
                        return;
                    }
                }
            }
            if (currentChatData.infoIDs.Count != 0)
            {
                foreach (var infoID in currentChatData.infoIDs)
                {
                    var infoData = ResourceManager.Inst.GetProfilerInfoData(infoID);
                    EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { infoData.category, infoData.id});
                    ProfileOverlaySystem.OnAdd?.Invoke(currentProfileData.overlayID);
                }

            }
            if (currentChatData.monologID != 0)
            {
                MonologSystem.OnStartMonolog?.Invoke(currentChatData.monologID, 0f, false);
            }
        }

        OnPointerEnter(eventData);
    }
}
