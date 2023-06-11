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


public class EmailSite : Site
{
    [ContextMenu("BindMailDataList")]
    public void BindMailDataList()
    {
        Transform parent = transform.Find("Center/MailPanel/Mails");
        
        foreach(Transform child in parent)
        {
            Mail mail = child.GetComponent<Mail>();
            if(mailDataList.Find(x => x == mail) != null)
            {
                continue;
            }
            mailDataList.Add(mail);
        }
    }

    private EEmailCategory currentCategory = EEmailCategory.Receive;

    [SerializeField]
    private List<Mail> mailDataList;
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

        GetMailSaveData();
        CreateLines();
        base.Init();
        ChangeEmailCategory();
        ShowMailLineAll();
    }

    private void GetMailSaveData()
    {
        return;
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
        foreach (Mail mail in mailDataList)
        {
            EmailLine emailLine = Instantiate(emailLinePrefab, emailLineParent);
            emailLine.Init(mail);
            ProfileOverlayOpenTrigger trigger = mail.GetComponent<ProfileOverlayOpenTrigger>();
            if(trigger != null)
            {
                emailLine.overlayTrigger = trigger;
                mail.OnOverlayClose += emailLine.overlayTrigger.Close;
            }
            emailLine.gameObject.SetActive(false);
            mail.Init();

            mail.OnChangeRemoveCatagory += emailLine.ChangeRemoveCategory;
            mail.OnChangeRemoveCatagory += (() => ChangeAlignCategory(EEmailCategory.Remove));
            
            baseEmailLineList.Add(emailLine);

            if (CheckCategoryData(mail, (int)EEmailCategory.Invisible))
            {
                mail.gameObject.SetActive(false);
            }
        }
        SettingReceiveMailCount();
    }

    private void SettingReceiveMailCount()
    {
        receiveMailCnt = 0;
        foreach(Mail mail in mailDataList)
        {
            if (CheckCategoryData(mail, (int)EEmailCategory.Remove)
                &&!!CheckCategoryData(mail, (int)EEmailCategory.Send))
            {
                if(CheckCategoryData(mail, (int)EEmailCategory.Invisible))
                {
                    if(DataManager.Inst.GetMailSaveData(mail.MailData.mailID) == null)
                    {
                        continue;
                    }
                }
                receiveMailCnt++;
            }
        }
        receiveMailCntText.text = receiveMailCnt.ToString();
    }
    
    private bool CheckCategoryData(int mailCategory, int checkCategory)
    {
        return mailCategory.ContainMask(checkCategory);
    }

    private bool CheckCategoryData(Mail mail, int checkCategory)
    {
        return mail.MailData.mailCategory.ContainMask(checkCategory);
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
        if (ps == null || !(ps[0] is int))
        {
            return;
        }
        int type = (int)ps[0];

        EmailLine line = baseEmailLineList.Find(x => x.MailData.mailID == type);
        float delay = 0f;
        if(ps.Length == 2)
        {
            delay = (ps[1] is float) ? (float)ps[1] : (int)ps[1];
        }
        await Task.Delay((int)(delay * 1000));
        if (line.Category.ContainMask((int)EEmailCategory.Invisible))
        {
            int num = line.Category.RemoveMask((int)EEmailCategory.Invisible);
            line.Category = num;
            DataManager.Inst.SetMailSaveData(type);
        }

        

        SetEmailCategory();
        SettingReceiveMailCount();
        
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
        currentMailLineList = 


        currentMailLineList = baseEmailLineList.OrderByDescending((x) => x.MailData.GetCompareFlagValue()).Where(n =>
        {
            int category = n.Category;
            bool flag1 = category.ContainMask((int)currentCategory);
            bool flag2 = category.ContainMask((int)EEmailCategory.Invisible) == false;
            if(flag2 == false)
            {
                flag2 = DataManager.Inst.GetMailSaveData(n.MailData.mailID) != null; 
            }
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
        //if (CheckGoogleLogin() == false)
        //{
        //    return;
        //}

        base.ShowSite();
    }
    protected override void HideSite()
    {
        base.HideSite();
    }

    protected override void ResetSite()
    {
        base.ResetSite();
    }

#if UNITY_EDITOR

    public void OnApplicationQuit()
    {   
        foreach(var mailLine in baseEmailLineList)
        {
            Debug.Log("¸ÞÀÏ0");
            mailLine.CurrentMail.DebugReset();
        }
    }
#endif
}
