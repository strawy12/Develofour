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

    public void Init()
    {
        EventManager.StartListening(EOutStarEvent.ClickFriendPanel, ChangeUserData);
    }

    private void ChangeUserData(object[] ps)
    {
        if(ps.Length < 1 || !(ps[0] is OutStarCharacterDataSO)) { return; }

        ChangeUserData(ps[0] as OutStarCharacterDataSO);
    }

    private void ChangeUserData(OutStarCharacterDataSO data)
    {
        OutStarCharacterDataSO userData = ResourceManager.Inst.GetOutStarProfileResourceManager(characterID);
        if(data != userData)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EOutStarEvent.ClickFriendPanel, ChangeUserData);
    }
#if UNITY_EDITOR
    private void PrefabSetting()
    {
        OutStarCharacterDataSO userData = ResourceManager.Inst.GetOutStarProfileResourceManager(characterID);

        foreach (var id in userData.timeChatIDList)
        {
            string lastMine = "";

            OutStarTimeChatDataSO timeChat = ResourceManager.Inst.GetOutStarTimeChatResourceManager(id); // to AssetDataBase
            string timeText = Define.GetOutStarTimeText(timeChat.time);
            TMP_Text timeTextObj = Instantiate(timeTextTemp, transform);
            timeTextObj.SetText(timeText);
            timeTextObj.gameObject.SetActive(true);

            UserChatBoxPanel currentPanel = null;

            foreach (var chatId in timeChat.chatDataIDList)
            {
                OutStarChatDataSO chatData = ResourceManager.Inst.GetOutStarChatResourceManager(chatId); // to AssetDataBase
                if (lastMine == "" || lastMine != chatData.isMine.ToString())
                {
                    if(chatData.isMine)
                    {
                        currentPanel = Instantiate(myChatBoxPanelTemp,transform);
                    }
                    else
                    {
                        currentPanel = Instantiate(otherChatBoxPanelTemp, transform);
                    }
                }
                currentPanel.AddChatBox(chatData);
                lastMine = chatData.isMine.ToString();
            }
        }
    }
#endif
}
