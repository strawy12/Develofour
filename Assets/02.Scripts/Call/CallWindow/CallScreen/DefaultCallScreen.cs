using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCallScreen : CallScreen
{
    private CallProfileDataSO callProfileData;

    public void Setting(CallProfileDataSO callProfileData)
    {
        this.callProfileData = callProfileData;
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(callProfileData.id);
        ProfileSetting(0, characterData.characterName, characterData.profileIcon);

        gameObject.SetActive(true);

        StartCall();
    }
    public override void StartCall()
    {
        base.StartCall();

        MonologSystem.AddOnEndMonologEvent(callProfileData.defaultCallID, SetSelectBtns);
        MonologSystem.OnStartMonolog?.Invoke(callProfileData.defaultCallID, false);

    }
    private void SetSelectBtns()
    {
        SetSelectBtns(callProfileData);
    }

    protected void SetSelectBtns(CallProfileDataSO callProfileData)
    {
        foreach (CallOption callOption in callProfileData.outGoingCallOptionList)
        {
            if (DataManager.Inst.IsSaveCallData(callOption.outGoingCallID)) continue;
            CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callOption.outGoingCallID);
            if (callData == null) continue;
            //저장 데이터 추가 해야함
            if (Define.NeedInfoFlag(callData.needInfoIDList))
            {
                MakeSelectBtn(callData, callOption.decisionName, () => SelectBtnCallAction(callData));
            }
        }
        CallDataSO notExistCallData = new CallDataSO();

        CallSelectButton instance = Instantiate(selectButton, selectBtnsTrm);
        instance.btnText.text = "통화 종료";
        buttonList.Add(instance);
        instance.btn.onClick.AddListener(() =>
        {
            HideSelectBtns();
            StopCall(true);
        });
    }
}
