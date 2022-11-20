using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public enum EMailType
{
    None = -1,
    Send,
    Receive,
    Remove
}

[Serializable]
public class Mail
{
    public EMailType mailType;
    public string nameText;
    public string informationText;
    public string timeText;
    public bool isHighlighted;
}

public enum EMailCategory
{
    None = -1,
    Receive,
    Highlighted,
    Send,
    Remove
}

public class EmailSite : Site
{
    private EMailCategory currentCategory = EMailCategory.Receive;

    [SerializeField]
    private List<Mail> baseMailList = new List<Mail>();

    [SerializeField]
    private List<Mail> currentMailList = new List<Mail>();

    [SerializeField]
    private EmailLine emailPrefab;

    [SerializeField]
    private Transform emailParent;

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
        base.Init();
        PopMail();
        ChangeEmailCategory(currentCategory);
        //TODO : 풀링제작
    }

    void Start()
    {
        Init();
    }

    #region Category Button Function
    private void ChangeReceiveEmail()
    {
        currentCategory = EMailCategory.Receive;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeHighlightEmail()
    {
        currentCategory = EMailCategory.Highlighted;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeSendEmail()
    {
        currentCategory = EMailCategory.Send;
        ChangeEmailCategory(currentCategory);
    }
    private void ChangeRemoveEmail()
    {
        currentCategory = EMailCategory.Remove;
        ChangeEmailCategory(currentCategory);
    }
    #endregion

    // 추후 위치를 변경해주기
    private void SuccessLogin(object o)
    {
        EventManager.StopListening(EQuestEvent.LoginGoogle, SuccessLogin);
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLogin = true;
    }

    private void ChangeEmailCategory(EMailCategory category)
    {
        switch(category)
        {
            case EMailCategory.Receive:
                {
                    //PushMail();
                    currentMailList.Clear();
                    currentMailList = baseMailList
                        .Where(n => n.mailType == EMailType.Receive).ToList();
                    PopMail();
                }
                break;

            case EMailCategory.Highlighted:
                {
                    //PushMail();
                    currentMailList.Clear();
                    currentMailList = baseMailList
                        .Where(n => n.mailType == EMailType.Receive && n.isHighlighted == true).ToList();
                    PopMail();
                }
                break;

            case EMailCategory.Send:
                {
                    //PushMail();
                    currentMailList.Clear();
                    currentMailList = baseMailList
                        .Where(n => n.mailType == EMailType.Send).ToList();
                    PopMail();
                }
                break;

            case EMailCategory.Remove:
                {
                    //PushMail();
                    currentMailList.Clear();
                    currentMailList = baseMailList
                        .Where(n => n.mailType == EMailType.Remove).ToList();
                    PopMail();
                }
                break;
        }
    }

    private void PopMail()
    {
        for (int i = 0; i < currentMailList.Count; i++)
        {
            EmailLine prefab = Instantiate(emailPrefab, emailParent);
            prefab.ChangeText(currentMailList[i].nameText, currentMailList[i].informationText, currentMailList[i].timeText);
            prefab.gameObject.SetActive(true);
        }
    }

    private void PushMail()
    {
        int num = emailParent.childCount;
        for(int i = 0; i < num; i++)
        {
            Destroy(emailParent.GetChild(1).gameObject);
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
            Browser.OnOpenSite(ESiteLink.GoogleLogin);
            EventManager.StartListening(EQuestEvent.LoginGoogle, SuccessLogin);
            return;
        }

        base.ShowSite();
    }
}
