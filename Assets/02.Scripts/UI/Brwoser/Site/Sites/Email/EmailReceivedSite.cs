using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mail
{
    public enum EMailType
    {
        None = -1,
        Send,
        Receive,
        Remove
    }
    public EMailType mailType;
    public string nameText;
    public string informationText;
    public string timeText;
    public bool isHighlighted;
}

public class EmailReceivedSite : Site
{
    [SerializeField]
    private List<Mail> mailList = new List<Mail>();

    [SerializeField]
    private EmailPrefab emailPrefab;

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
            EmailPrefab prefab = Instantiate(emailPrefab, emailParent);
            prefab.ChangeText(mailList[i].nameText, mailList[i].informationText, mailList[i].timeText);
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
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin)
        {
            Browser.OnOpenSite(ESiteLink.GoogleLogin, 0f);
            EventManager.StartListening(EQuestEvent.LoginGoogle, SuccessLogin);
            return;
        }

        base.ShowSite();
    }
}
