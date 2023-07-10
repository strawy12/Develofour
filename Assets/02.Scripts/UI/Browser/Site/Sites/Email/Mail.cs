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

    public Action OnOverlayClose;

    public ProfileOverlayOpenTrigger overlayTrigger;

    public Action OnOpenMail;

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
        overlayTrigger = GetComponent<ProfileOverlayOpenTrigger>();
        if(overlayTrigger != null)
        {
            OnOverlayClose += overlayTrigger.Close;
            OverlayOpenEventAdd();
        }
        OnOpenMail += ShowMail;
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

        if (overlayTrigger != null)
        {
            overlayTrigger.Open();
        }

        gameObject.SetActive(true);
    }

    public virtual void HideMail()
    {
        OnOverlayClose?.Invoke();
        gameObject.SetActive(false);
    }

    protected virtual void DestroyMail()
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

    private void OverlayOpenEventAdd()
    {
        Browser.currentBrowser.OnSelected += OverlayOpen;
    }

    private void OverlayOpen()
    {
        if (this.gameObject.activeSelf)
        {
            if (overlayTrigger != null)
            {
                overlayTrigger.Open();
            }
        }
    }

    public void DebugReset()
    {
        mailData.mailCategory = originMask;
    }

    private void OnDisable()
    {
        OnOverlayClose?.Invoke();
    }

    private void OnDestroy()
    {
        OnDisable();
    }
}
