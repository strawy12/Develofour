using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;
using ExtenstionMethod;
using TMPro;
public enum EEmailCategory
{
    None = 0x00,      // 00000000
    Receive = 0x01,   // 00000001
    Favorite = 0x02,  // 00000010
    Send = 0x04,      // 00000100
    Remove = 0x08,    // 00001000
    Invisible = 0x10, // 00010000
}

public enum EMailType
{
    Default = 0,
    Writer = 100,
    WriterDummy1,   
    WriterDummy2,
    WriterDummy3,
    WriterDummy4,
    WriterDummy5,
    WriterDummy6,
    WriterDummy7,
    WriterDummy8,
    WriterDummy9, 
    WriterDummy10,
    WriterDummy11,
    WriterDummy12,
    WriterDummy13,
    WriterSendMail1,
    WriterSendMail2,
    WriterSendMail3,//DummyEnd 
    WriterBusinessMail1,
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

    private int newMailNumber = 0;

    [SerializeField]
    private TextMeshProUGUI receiveMailCntText;
    private int receiveMailCnt = 0;

    public Action miniGameClear;

    public override void Init()
    {
        receiveBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Receive));
        favoriteBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Favorite));
        sendBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Send));
        removeBtn.onClick.AddListener(() => ChangeAlignCategory(EEmailCategory.Remove));

        EventManager.StartListening(EMailSiteEvent.VisiableMail, VisiableMail);
        EventManager.StartListening(EMailSiteEvent.RefreshPavoriteMail, FavoriteRefreshMail);
        currentCategory = EEmailCategory.Receive;

        CreateLines();
        base.Init();
        ChangeEmailCategory();
        ShowMailLineAll();
    }

    private void ClearPoliceMiniGame(object[] ps)
    {
        Debug.Log("?????? ??? ???????");
        SettingReceiveMailCount();
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
        SettingReceiveMailCount();
    }

    private void SettingReceiveMailCount()
    {
        receiveMailCnt = 0;
        foreach(MailData data in mailDataList)
        {
            if (!data.mailDataSO.mailCategory.ContainMask((int)EEmailCategory.Remove)
                &&!data.mailDataSO.mailCategory.ContainMask((int)EEmailCategory.Invisible)
                &&!data.mailDataSO.mailCategory.ContainMask((int)EEmailCategory.Send))
            {
                receiveMailCnt++;
            }
        }
        receiveMailCntText.text = receiveMailCnt.ToString();
    }

    private void ChangeAlignCategory(EEmailCategory category)
    {
        currentCategory = category;
        ChangeEmailCategory();
    }

    private void FavoriteRefreshMail(object[] emptyObj)
    {
        ChangeAlignCategory(currentCategory);
    }

    private async void VisiableMail(object[] ps)
    {
        if (ps == null || !(ps[0] is EMailType))
        {
            Debug.LogError("????????? Param??? null????????? Type??? ??????????????????.");
            return;
        }
        EMailType type = (EMailType)ps[0];

        EmailLine line = baseEmailLineList.Find(x => x.MailData.Type == type);
        line.mailNumber = newMailNumber++;
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
        SettingReceiveMailCount();
    }

    private void SuccessLogin(object[] o)
    {
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite = true;
    }

    private void ChangeEmailCategory()
    {
        HideAllMail();
        HideAMailLineAll();

        SetEmailCategory();
        SettingReceiveMailCount();
    }

    private void SetEmailCategory()
    {
        
        currentMailLineList = baseEmailLineList.OrderByDescending((x) => x.MailData.Month).ThenByDescending((x) => x.MailData.Day).ThenByDescending((x)=>x.mailNumber).Where(n =>
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
            currentMailLineList[i].transform.SetSiblingIndex(i);
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


    // ?????? ?????? ??????
    private bool CheckGoogleLogin()
    {
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite)
        {
            EventManager.TriggerEvent(ELoginSiteEvent.RequestSite, new object[] { ESiteLink.Email });
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin, Constant.LOADING_DELAY , false});
            
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
        Debug.Log("MailData Category ????????? ?????? ?????? ????????? ????????? ???????????? ????????????.");

        foreach(var mailLine in baseEmailLineList)
        {
            mailLine.CurrentMail.DebugReset();
        }
    }
}
