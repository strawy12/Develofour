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
    [SerializeField]
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
            mails[i].mail.Init();
            baseEmailLineList.Add(emailLine);

        }
        SetEmailCategory();
    }
    #region Category Button Function
    private void ChangeReceiveEmail()
    {
        currentCategory = EEmailCategory.Receive;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeHighlightEmail()
    {
        if(currentCategory == EEmailCategory.Favorite)
        {
            currentCategory = EEmailCategory.Receive;
        }
        else
        {
            currentCategory = EEmailCategory.Favorite;
        }
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

    private void SetEmailCategory()
    {
        for(int i = 0; i < baseEmailLineList.Count; i++)
        {
            baseEmailLineList[i].SetEmailCategory();
        }
    }

    private void ChangeEmailCategory(EEmailCategory category)
    {
        HideMail();
        HideMailLine();
        SetEmailCategory();
        switch (category)
        {
            case EEmailCategory.Receive:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Receive
                        || n.emailCategory == EEmailCategory.Favorite).ToList();
                    Debug.Log("receive" + currentMailLineList.Count);
                }
                break;
                
            case EEmailCategory.Favorite:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Favorite).ToList();
                }
                break;

            case EEmailCategory.Send:
                {
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Send).ToList();
                }
                break;

            case EEmailCategory.Remove:
                {
                    Debug.Log("1");
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Remove).ToList();
                    Debug.Log("remove " + currentMailLineList.Count);   
                }
                break;
        }
        ShowMailLine();
    }

    private void HideMail()
    {
        var mails = from mailLine in baseEmailLineList
                    where mailLine.mail.isActiveAndEnabled == true
                    select mailLine;

        foreach(EmailLine mailLine in mails)
        {
            mailLine.mail.HideMail();
        }
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
            EventManager.TriggerEvent<EBrowserEvent>(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin });
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
