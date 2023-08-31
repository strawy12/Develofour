using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfilerUserCallPanel : MonoBehaviour, IPointerClickHandler
{
    private CharacterInfoDataSO characterData;

    [SerializeField]
    private TMP_Text userName;

    [SerializeField]
    private Image userImage;

    [SerializeField]
    private Button callBtn;

    private ProfilerCallKeyPad keyPad;

    public void Init(string characterNumber, ProfilerCallKeyPad keyPad)
    {
        this.keyPad = keyPad;
        characterData = ResourceManager.Inst.FindCharacterPhoneNumber(characterNumber);

        userName.SetText(characterData.characterName);
        userImage.sprite = characterData.profileIcon;
        gameObject.SetActive(true);

        callBtn.onClick.AddListener(CallStart);

    }

    private void CallStart()
    {
        CallSystem.OnOutGoingCall?.Invoke(characterData.id);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        keyPad.SetNumberText(characterData.phoneNum);
    }


}
