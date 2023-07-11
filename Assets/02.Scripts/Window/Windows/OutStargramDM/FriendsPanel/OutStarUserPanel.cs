using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
public class OutStarUserPanel : MonoBehaviour, IPointerClickHandler
{
    private OutStarCharacterDataSO outStarCharacterData;
    private CharacterInfoDataSO characterData;

    public string CharacterID { get => outStarCharacterData.ID; }

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text lastChatText;

    [SerializeField]
    private GameObject selectedImage;

    [SerializeField]
    private Image userIconImage;

    [SerializeField]
    private GameObject pointerPanel;

    public void Init(OutStarCharacterDataSO characterData)
    {
        outStarCharacterData = characterData;
        this.characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(characterData.ID);
        SettingPanel();
    }
    
    private void SettingPanel()
    {
        nameText.text = characterData.characterName;
        List<OutStarTimeChatDataSO> timeChatList = new List<OutStarTimeChatDataSO>();

        OutStarTimeChatDataSO lastTimeChat = null;
        foreach(var timeChatID in outStarCharacterData.timeChatIDList)
        {
            OutStarTimeChatDataSO timeChat = ResourceManager.Inst.GetResource<OutStarTimeChatDataSO>(timeChatID);
            timeChatList.Add(timeChat);
        }

        lastTimeChat = timeChatList.OrderBy(x => x.time).LastOrDefault();
        if(lastTimeChat != null && lastTimeChat.chatDataIDList != null)
        {
            OutStarChatDataSO lastChatData = ResourceManager.Inst.GetResource<OutStarChatDataSO>(lastTimeChat.chatDataIDList.LastOrDefault());
            if(lastChatData != null)
            {
                lastChatText.text = lastChatData.chatText;
            }
        }
    }

    public void SetActiveSelectedImage(bool isActive)
    {
        selectedImage.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EOutStarEvent.ClickFriendPanel, new object[1] { outStarCharacterData });
    }
}
