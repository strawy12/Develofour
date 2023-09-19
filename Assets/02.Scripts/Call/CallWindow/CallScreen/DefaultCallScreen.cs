using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCallScreen : CallScreen
{
    private CallProfileDataSO currentCallProfileData;

    public void Setting(CallProfileDataSO callProfileData)
    {
        currentCallProfileData = callProfileData;

        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(currentCallProfileData.id);
        Debug.Log(characterData.id + characterData.characterName);

        gameObject.SetActive(true);

        userImageList[0].nameText.SetText(characterData.characterName);
        userImageList[0].profileImage.sprite = characterData.profileIcon;

        StartCall();
    }

    public override void StartCall()
    {
        base.StartCall();

        MonologSystem.AddOnEndMonologEvent(currentCallProfileData.defaultCallID, SetSelectBtns);
        MonologSystem.OnStartMonolog?.Invoke(currentCallProfileData.defaultCallID, false);
    }

    private void SetSelectBtns()
    {
        SetSelectBtns(currentCallProfileData);
    }

    protected void SetSelectBtns(CallProfileDataSO callProfileData)
    {
        foreach (CallOption callOption in callProfileData.outGoingCallOptionList)
        {
            if (DataManager.Inst.IsSaveCallData(callOption.outGoingCallID)) continue;
            CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callOption.outGoingCallID);
            if (callData == null) continue;
            //���� ������ �߰� �ؾ���
            if (Define.NeedInfoFlag(callData.needInfoIDList))
            {
                MakeSelectBtn(callData, callOption.decisionName, () => SelectBtnCallAction(callData));
            }
        }
        CallDataSO notExistCallData = new CallDataSO();

        CallSelectButton instance = Instantiate(selectButton, selectBtnsTrm);
        instance.btnText.text = "��ȭ ����";
        buttonList.Add(instance);
        instance.btn.onClick.AddListener(() =>
        {
            HideSelectBtns();
            StopCall(true);
        });
    }
    public override void StopCall(bool isClose)
    {
        userImageList[0].profileImage.sprite = null;
        userImageList[0].nameText.SetText("");
        currentCallProfileData = null;
        base.StopCall(isClose);
    }
}
