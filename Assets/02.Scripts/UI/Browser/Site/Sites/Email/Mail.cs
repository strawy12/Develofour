using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mail : MonoBehaviour
{
    #region 버튼들
    [SerializeField]
    private HighlightBtn mailCloseButton;

    [SerializeField]
    private HighlightBtn mailDestroyButton;

    [SerializeField]
    private EmailFavoriteButton mailFavoriteButton;

    #endregion
    [SerializeField]
    protected MailDataSO mailData;

    public MailDataSO MailData { get { return mailData; } }

    public Action OnChangeRemoveCatagory;

    [SerializeField]
    private Image userProfileImage;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private TMP_Text receiveText;

    [SerializeField]
    private TMP_Text sendText;

    [SerializeField]
    private TMP_Text timeText;

    private int originMask;

    [ContextMenu("BindBtns")]
    public void BindBtns()
    {
        mailCloseButton = transform.Find("TopBar/BackButton").GetComponent<HighlightBtn>();
        mailDestroyButton = transform.Find("TopBar/TrashButton").GetComponent<HighlightBtn>();
    }


    public virtual void Init()
    {
        originMask = mailData.mailCategory;
        mailCloseButton.OnClick +=(HideMail);
        mailDestroyButton.OnClick +=(DestroyMail);
        mailFavoriteButton.Init(mailData.isFavorited);
    }

    public virtual void ShowMail()
    {
        userProfileImage.sprite = mailData.userProfile;
        titleText.text = mailData.titleText;
        receiveText.text = mailData.receiveName + "에게";
        sendText.text = mailData.sendName;
        MailSaveData saveData = DataManager.Inst.GetMailSaveData(mailData.mailID);
        if (saveData != null)
        {
            timeText.text = $"{mailData.Year}. {saveData.month}. {saveData.day}. {saveData.hour}:{saveData.minute}";
        }
        else
        {
            timeText.text = mailData.TimeText;
        }
        gameObject.SetActive(true);
    }

    public virtual void HideMail()
    {
        gameObject.SetActive(false);
    }

    public virtual void DestroyMail()
    {
        mailData.mailCategory |= (int)EEmailCategory.Remove;
        OnChangeRemoveCatagory?.Invoke();
        HideMail();
    }

    public void FavoriteMail(bool isFavorited)
    {
        if(isFavorited)
        {
            mailData.mailCategory |= (int)EEmailCategory.Favorite;
        }
        else
        {
            mailData.mailCategory = mailData.mailCategory.RemoveMask((int)EEmailCategory.Favorite);
            EventManager.TriggerEvent(EMailSiteEvent.RefreshPavoriteMail);
        }
    }

    public void DebugReset()
    {
        mailData.mailCategory = originMask;
    }
}
