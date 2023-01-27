using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mail : MonoBehaviour
{
    #region ¹öÆ°µé
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
    private bool isCanDelete = true;

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
        gameObject.SetActive(true);
    }

    public virtual void HideMail()
    {
        Debug.Log(11);
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
