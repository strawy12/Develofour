using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum EMailType
{
    None = -1,
    Send,
    Receive,
    Remove
}

public class EmailSite : Site
{
    [SerializeField]
    private List<MailDataSO> mailList = new List<MailDataSO>();

    [SerializeField]
    private EmailLine emailPrefab;

    [SerializeField]
    private Transform emailParent;

    public override void Init()
    {
        base.Init();
        LoadingMail();
        //TODO : 풀링제작
    }

    // 추후 위치를 변경해주기
    private void SuccessLogin(object o)
    {
        Debug.Log(11);
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin = true;
    }

    public void LoadingMail()
    {
        for (int i = 0; i < mailList.Count; i++)
        {
            EmailLine prefab = Instantiate(emailPrefab, emailParent);
            prefab.ChangeText(mailList[i].nameText, mailList[i].informationText, mailList[i].timeText, mailList[i].mailObject);
        }
    }

    protected override void HideSite()
    {
        base.HideSite();
    }

    protected override void ResetSite()
    {
        base.ResetSite();
    }

    protected override void ShowSite()
    {
        if(CheckGoogleLogin() == false) {
            return;
        }
        base.ShowSite();
    }

    private bool CheckGoogleLogin()
    {
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin)
        {
            Browser.OnOpenSite(ESiteLink.GoogleLogin);
            EventManager.StartListening(EQuestEvent.LoginGoogle, SuccessLogin);
            return false;
        }

        return true;

    }
}
