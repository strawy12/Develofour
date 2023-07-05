using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        this.characterData = ResourceManager.Inst.GetCharacterDataSO(characterData.ID);
        SettingPanel();
    }
    
    private void SettingPanel()
    {
        nameText.text = characterData.characterName;
        //lastChatText
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
