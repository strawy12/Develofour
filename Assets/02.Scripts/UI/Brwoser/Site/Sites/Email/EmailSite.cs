using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;
using ExtenstionMethod;

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
    PoliceReply,
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
        EventManager.StartListening(EMailSiteEvent.VisiableMail, VisiableMail);

        currentCategory = EEmailCategory.Receive;

        CreateLines();
        base.Init();
        ChangeEmailCategory();
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
            data.mail.OnChangeRemoveCatagory += (() => ChangeAlignCategory(EEmailCategory.Remove));

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
        ChangeEmailCategory();
    }

    private async void VisiableMail(object[] ps)
    {
        if (ps == null || !(ps[0] is EMailType))
        {
            Debug.LogError("들어온 Param이 null이거나 Type이 맞지않습니다.");
            return;
        }
        EMailType type = (EMailType)ps[0];

        EmailLine line = baseEmailLineList.Find(x => x.MailData.Type == type);

        float delay = 0f;
        if(ps.Length == 2)
        {
            delay = (ps[1] is float) ? (float)ps[1] : (int)ps[1];
        }

        await Task.Delay((int)(delay * 1000));

        if (line.Category.ContainMask((int)EEmailCategory.Invisible))
        {
            line.Category = line.Category.RemoveMask((int)EEmailCategory.Invisible);
        }

        SetEmailCategory();
    }

    private void SuccessLogin(object[] o)
    {
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin = true;
    }

    private void ChangeEmailCategory()
    {
        HideAllMail();
        HideAMailLineAll();

        SetEmailCategory();
    }

    private void SetEmailCategory()
    {
        currentMailLineList = baseEmailLineList.Where(n =>
        {
            int category = n.Category;
            bool flag1 = category.ContainMask((int)currentCategory);
            bool flag2 = category.ContainMask((int)EEmailCategory.Invisible) == false;
            bool flag3 = (currentCategory != EEmailCategory.Remove && category.ContainMask((int)EEmailCategory.Remove)) == false;

            return flag1 && flag2 && flag3;

        }).ToList();

        ShowMailLineAll();
    }

    private void HideAllMail()
    {
        var mails = from mailLine in baseEmailLineList
                    where mailLine.IsActiveAndEnabled == true
                    select mailLine;

        foreach (EmailLine mailLine in mails)
        {
            mailLine.HideMail();
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

    public void OnApplicationQuit()
    {
        Debug.Log("MailData Category 저장을 하지 않는 디버그 코드가 실행중에 있습니다.");

        foreach(var mailLine in baseEmailLineList)
        {
            mailLine.CurrentMail.DebugReset();
        }
    }
}
