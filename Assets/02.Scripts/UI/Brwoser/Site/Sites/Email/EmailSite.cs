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
    Highlighted,
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

    [SerializeField]
    private List<EmailLine> baseEmailLineList = new List<EmailLine>();

    private List<EmailLine> currentMailLineList = new List<EmailLine>();

    [SerializeField]
    private EmailLine emailLinePrefab;

    [SerializeField]
    private Transform emailLineParent;

    #region Category Buttons
    [SerializeField]
    private Button receiveCategory;
    [SerializeField]
    private Button highlightCategory;
    [SerializeField]
    private Button sendCategory;
    [SerializeField]
    private Button removeCategory;
    #endregion

    public override void Init()
    {
        receiveCategory.onClick.AddListener(ChangeReceiveEmail);
        highlightCategory.onClick.AddListener(ChangeHighlightEmail);
        sendCategory.onClick.AddListener(ChangeSendEmail);
        removeCategory.onClick.AddListener(ChangeRemoveEmail);

        for(int i = 0; i < mails.Count; i++)
        {
            EmailLine emailLine = Instantiate(emailLinePrefab, emailLineParent);
            emailLine.Init(mails[i].mailDataSO, mails[i].mail);
            emailLine.gameObject.SetActive(false);
            baseEmailLineList.Add(emailLine);
        }

        base.Init();
        ChangeEmailCategory(currentCategory);
        ShowMailLine();
    }

    void Start()
    {
        Init();
    }

    #region Category Button Function
    private void ChangeReceiveEmail()
    {
        currentCategory = EEmailCategory.Receive;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeHighlightEmail()
    {
        currentCategory = EEmailCategory.Highlighted;
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
        switch(category)
        {
            case EEmailCategory.Receive:
                {
                    HideMailLine();
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Receive).ToList();
                    ShowMailLine(); 
                }
                break;

            case EEmailCategory.Highlighted:
                {
                    HideMailLine();
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Receive && n.IsHighrighted == true).ToList();
                    ShowMailLine();
                }
                break;

            case EEmailCategory.Send:
                {
                    HideMailLine();
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Send).ToList();
                    ShowMailLine();
                }
                break;

            case EEmailCategory.Remove:
                {
                    HideMailLine();
                    currentMailLineList = baseEmailLineList.Where(n => n.emailCategory == EEmailCategory.Remove).ToList();
                    ShowMailLine();
                }
                break;
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
