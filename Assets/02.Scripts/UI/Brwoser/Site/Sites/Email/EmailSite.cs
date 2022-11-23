using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public enum EEmailCategory
{
    None = -1,
    Receive,
    Favorite,
    Send,
    Remove
}
[Serializable]
public class MailData
{
    public MailDataSO mailDataSO;
    public Mail mail;
}


public class EmailSite : Site
{
    private EEmailCategory currentCategory = EEmailCategory.Receive;

    [SerializeField]
    private List<MailData> mails = new List<MailData>();

    private List<EmailLine> baseEmailLineList = new List<EmailLine>();

    private List<EmailLine> currentMailLineList = new List<EmailLine>();

    [SerializeField]
    private EmailLine emailLinePrefab;

    [SerializeField]
    private Transform emailLineParent;

    #region Category Buttons
    [SerializeField]
    private Button receiveBtn;
    [SerializeField]
    private Button favoriteBtn;
    [SerializeField]
    private Button sendBtn;
    [SerializeField]
    private Button removeBtn;
    #endregion

    public override void Init()
    {
        receiveBtn.onClick.AddListener(ChangeReceiveEmail);
        favoriteBtn.onClick.AddListener(ChangeHighlightEmail);
        sendBtn.onClick.AddListener(ChangeSendEmail);
        removeBtn.onClick.AddListener(ChangeRemoveEmail);

        CreateLine();
        base.Init();
        ChangeEmailCategory(currentCategory);
        ShowMailLine();
    }
    private void CreateLine()
    {
        for (int i = 0; i < mails.Count; i++)
        {
            EmailLine emailLine = Instantiate(emailLinePrefab, emailLineParent);
            emailLine.Init(mails[i].mailDataSO, mails[i].mail);
            emailLine.gameObject.SetActive(false);
            baseEmailLineList.Add(emailLine);
        }

    }
    #region Category Button Function
    private void ChangeReceiveEmail()
    {
        currentCategory = EEmailCategory.Receive;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeHighlightEmail()
    {
        currentCategory = EEmailCategory.Favorite;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeSendEmail()
    {
        currentCategory = EEmailCategory.Send;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeRemoveEmail()
    {
        currentCategory = EEmailCategory.Remove;
        ChangeEmailCategory(currentCategory);
    }
    #endregion

    private void SuccessLogin(object o)
    {
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin = true;
    }

    private void ChangeEmailCategory(EEmailCategory category)
    {
        HideMailLine();
        switch (category)
        {
            case EEmailCategory.Receive:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Receive).ToList();
                }
                break;

            case EEmailCategory.Favorite:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Receive && n.IsFavorited == true).ToList();
                }
                break;

            case EEmailCategory.Send:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Send).ToList();
                }
                break;

            case EEmailCategory.Remove:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Remove).ToList();
                }
                break;
        }
        ShowMailLine();
    }

    private void ShowMailLine()
    {
        for (int i = 0; i < currentMailLineList.Count; i++)
        {
            currentMailLineList[i].gameObject.SetActive(true);
        }
        currentMailLineList.Clear();
    }

    private void HideMailLine()
    {
        foreach(EmailLine emailLine in baseEmailLineList)
        {
            emailLine.gameObject.SetActive(false);
        }
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
    protected override void HideSite()
    {
        base.HideSite();
    }

    protected override void ResetSite()
    {
        base.ResetSite();
    }
}
