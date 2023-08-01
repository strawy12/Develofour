using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerCallUserListPanel : MonoBehaviour
{
    [SerializeField]
    private ProfilerUserCallPanel userCallPanelTemp;
    [SerializeField]
    private Transform userCallParent;
    private List<CharacterInfoDataSO> characterDataList;
    private ProfilerCallKeyPad keyPad;
    private List<string> phoneNumberList;

    public void Init(ProfilerCallKeyPad keyPad)
    {
        this.keyPad = keyPad;
        phoneNumberList = new List<string>();
  
        characterDataList = ResourceManager.Inst.GetResourceList<CharacterInfoDataSO>();
        EventManager.StartListening(ECallEvent.AddAutoCompleteCallBtn, AddUserCallProfile);

        foreach (var data in characterDataList)
        {
            if (DataManager.Inst.IsSavePhoneNumber(data.phoneNum))
            {
                phoneNumberList.Add(data.phoneNum);
                CreateCallProfile(data.phoneNum);
            }
        }
    }

    private void AddUserCallProfile(object[] ps)
    {
        if (!(ps[0] is string))
        {
            return;
        }

        string number = ps[0] as string;
        if(ResourceManager.Inst.FindCharacterPhoneNumber(number).id == Constant.CharacterKey.MISSING)
        {
            return;
        }

        Debug.Log(DataManager.Inst.IsSavePhoneNumber(number));
        if (DataManager.Inst.IsSavePhoneNumber(number))
        {
            return;
        }

        if (!phoneNumberList.Contains(number))
        {
            phoneNumberList.Add(number);
        }
        CreateCallProfile(number);

        DataManager.Inst.AddSavePhoneNumber(number);
    }
    private void CreateCallProfile(string number)
    {
        ProfilerUserCallPanel userCallPanel = Instantiate(userCallPanelTemp, userCallParent);
        userCallPanel.Init(number, keyPad);
        userCallPanel.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(ECallEvent.AddAutoCompleteCallBtn, AddUserCallProfile);
    }
}
