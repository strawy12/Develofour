using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public enum EEmailCategory
{
    None = 0x00, // 00000000
    Receive = 0x01, // 00000001
    Favorite = 0x02, // 00000010
    Send = 0x04, // 00000100
    Remove = 0x08, // 00001000
    Invisible = 0x10, // 00010000
}

public enum EMailType
{
    Default,
    PoliceAttendance,
    BlogDelete,
    SnsPasswordChange,
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
    private List<MailData> mailDataList;
    [SerializeField]
    private EmailLine emailLinePrefab;
    [SerializeField]
    private Transform emailLineParent;

    //private Dictionary<EMailType, MailData> mailDataDictionary = new Dictionary<EMailType, MailData>();
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

    private List<EmailLine> baseEmailLineList = new List<EmailLine>();
    private List<EmailLine> currentMailLineList = new List<EmailLine>();

    public override void Init()
    {
        receiveBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Receive));
        favoriteBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Favorite));
        sendBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Send));
        removeBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Remove));
        EventManager.StartListening(EGamilSiteEvent.SendMail, null);


        //RegisterMailData();

        CreateLines();
        base.Init();
        ChangeEmailCategory(currentCategory);
        ShowMailLineAll();
    }

    //private void RegisterMailData()
    //{
    //    mailDataDictionary = new Dictionary<EMailType, MailData>();

    //    foreach(var mailData in mailDataList.MailDataList)
    //    {
    //        mailDataDictionary.Add(mailData.mailDataSO.Type, mailData);
    //    }
    //}

    private void CreateLines()
    {
        foreach (MailData data in mailDataList)
        {
            EmailLine emailLine = Instantiate(emailLinePrefab, emailLineParent);
            emailLine.Init(data.mailDataSO, data.mail);
            emailLine.gameObject.SetActive(false);
            data.mail.Init();

            data.mail.OnChangeRemoveCatagory += emailLine.ChangeRemoveCategory;
            data.mail.OnChangeRemoveCatagory += (() => ChangeEmailCategory(EEmailCategory.Remove));

            baseEmailLineList.Add(emailLine);

            if (data.mailDataSO.mailCategory.ContainMask((int)EEmailCategory.Invisible))
            {
                data.mail.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeAlignCategory(EEmailCategory category)
    {
        currentCategory = category;
        ChangeEmailCategory(currentCategory);
    }

    private void ReceiveEmail(object[] ps)
    {
        
    }

    private void SuccessLogin(object[] o)
    {
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin = true;
    }

    private void ChangeEmailCategory(EEmailCategory category)
    {
        HideAllMail();
        HideAMailLineAll();

        currentMailLineList = baseEmailLineList.Where(n =>
        {
            return n.Category.ContainMask((int)category)
            &&     n.Category.ContainMask((int)EEmailCategory.Invisible) == false;

        }).ToList();

        ShowMailLineAll();
    }

    private void HideAllMail()
    {
        var mails = from mailLine in baseEmailLineList
                    where mailLine.mail.isActiveAndEnabled == true
                    select mailLine;

        foreach (EmailLine mailLine in mails)
        {
            mailLine.mail.HideMail();
        }
    }

    private void ShowMailLineAll()
    {
        for (int i = 0; i < currentMailLineList.Count; i++)
        {
            currentMailLineList[i].gameObject.SetActive(true);
        }
        currentMailLineList.Clear();
    }

    private void HideAMailLineAll()
    {
        foreach (EmailLine emailLine in baseEmailLineList)
        {
            emailLine.gameObject.SetActive(false);
        }
    }

    protected override void ShowSite()
    {
        if (CheckGoogleLogin() == false)
        {
            return;
        }

        base.ShowSite();
    }


    // 추후 위치 변경
    private bool CheckGoogleLogin()
    {
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin)
        {
            EventManager.TriggerEvent(ELoginSiteEvent.RequestSite, new object[] { ESiteLink.Email });
            EventManager.TriggerEvent<EBrowserEvent>(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin, Constant.LOADING_DELAY });
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
