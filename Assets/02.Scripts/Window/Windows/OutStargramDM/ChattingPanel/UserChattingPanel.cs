using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserChattingPanel : MonoBehaviour
{
    public string characterID;

    [SerializeField]
    private UserChatBoxPanel myChatBoxPanelTemp;

    [SerializeField]
    private UserChatBoxPanel otherChatBoxPanelTemp;

    [SerializeField]
    private TMP_Text timeTextTemp;

    public ProfileOverlayOpenTrigger overlayOpenTrigger;

    public void Init()
    {
        overlayOpenTrigger = GetComponent<ProfileOverlayOpenTrigger>();
        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeUserData);
    }

    private void ChangeUserData(object[] ps)
    {
        if (ps.Length < 1 || !(ps[0] is OutStarProfileDataSO)) { return; }

        ChangeUserData(ps[0] as OutStarProfileDataSO);
    }

    private void ChangeUserData(OutStarProfileDataSO data)
    {
        OutStarProfileDataSO userData = ResourceManager.Inst.GetResource<OutStarProfileDataSO>(characterID);
        if (data != userData)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        overlayOpenTrigger.Open();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        overlayOpenTrigger.Close();
    }
    private void OnDisable()
    {
        overlayOpenTrigger.Close();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeUserData);
    }
#if UNITY_EDITOR
    public void PrefabSetting()
    {
        OutStarProfileDataSO userData = Define.GuidsToList<OutStarProfileDataSO>("t:OutStarProfileDataSO").Find(x=>x.id == characterID);

        foreach (var id in userData.timeChatIDList)
        {
            string lastMine = "";

            OutStarTimeChatDataSO timeChat = Define.GuidsToList<OutStarTimeChatDataSO>("t:OutStarTimeChatDataSO").Find(x=>x.id == id);
            if (timeChat == null) {
                Debug.Log($"timeChat {id} is null");
                continue;
            }
            string timeText = Define.GetOutStarTimeText(timeChat.time);
            TMP_Text timeTextObj = Instantiate(timeTextTemp, transform);
            
            timeTextObj.SetText(timeText);
            timeTextObj.gameObject.SetActive(true);

            UserChatBoxPanel currentPanel = null;

            foreach (var chatId in timeChat.chatDataIDList)
            {
                OutStarChatDataSO chatData = Define.GuidsToList<OutStarChatDataSO>("t:OutStarChatDataSO").Find(x=>x.id == chatId);
                if (chatData == null)
                {
                    Debug.Log($"chat id:{chatId} is null");
                    continue;
                }
                if (lastMine == "" || lastMine != chatData.isMine.ToString())
                {
                    if (chatData.isMine)
                    {
                        currentPanel = Instantiate(myChatBoxPanelTemp, transform);
                    }
                    else
                    {
                        currentPanel = Instantiate(otherChatBoxPanelTemp, transform);
                    }
                }
                currentPanel.gameObject.SetActive(true);
                currentPanel.AddChatBox(chatData);
                lastMine = chatData.isMine.ToString();
            }
        }
    }
#endif
}
