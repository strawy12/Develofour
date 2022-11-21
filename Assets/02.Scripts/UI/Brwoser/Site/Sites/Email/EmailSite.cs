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
    private List<Mail> baseMailDataList = new List<Mail>();

    [SerializeField]
    private List<GameObject> baseMailList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> currentMailList = new List<GameObject>();

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

        //베이스 메일 리스트 모두 생성시키기

        for(int i = 0; i < baseMailDataList.Count; i++)
        {
            EmailLine prefab = Instantiate(emailPrefab, emailParent);
            prefab.ChangeText(baseMailDataList[i].nameText, baseMailDataList[i].informationText, baseMailDataList[i].timeText);
            prefab.gameObject.SetActive(false);
            baseMailList[i] = prefab.gameObject;
        }

        base.Init();
        ShowMail();
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
                    PushMail();
                    currentMailList = baseMailList
                        .Where(n => n.GetComponent<Mail>().mailType == EMailType.Receive).ToList();
                    ShowMail();
                }
                break;

            case EMailCategory.Highlighted:
                {
                    PushMail();
                    currentMailList = baseMailList
                        .Where(n => n.GetComponent<Mail>().mailType == EMailType.Receive && n.GetComponent<Mail>().isHighlighted == true).ToList();
                    ShowMail();
                }
                break;

            case EMailCategory.Send:
                {
                    PushMail();
                    currentMailList = baseMailList
                        .Where(n => n.GetComponent<Mail>().mailType == EMailType.Send).ToList();
                    ShowMail();
                }
                break;

            case EMailCategory.Remove:
                {
                    PushMail();
                    currentMailList = baseMailList
                        .Where(n => n.GetComponent<Mail>().mailType == EMailType.Remove).ToList();
                    ShowMail();
                }
                break;
        }
    }

    private void ShowMail()
    {
        for (int i = 0; i < currentMailList.Count; i++)
        {
            currentMailList[i].SetActive(true);
        }
        currentMailList.Clear();
    }

    private void PushMail()
    {
        for(int i = 0; i < baseMailList.Count; i++)
        {
            baseMailList[i].SetActive(false);
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
