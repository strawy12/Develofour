using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mail : MonoBehaviour
{
    #region ¹öÆ°µé
    [SerializeField]
    private Button mailCloseButton;

    [SerializeField]
    private Button mailDestroyButton;

    [SerializeField]
    private Button mailFavoriteButton;
    #endregion
    [SerializeField]
    protected EEmailCategory mailType;

    [SerializeField]
    protected MailDataSO mailData;

    public MailDataSO MailData { get { return mailData; } }

    public virtual void Init()
    {
        mailCloseButton.onClick.AddListener(HideMail);
        mailDestroyButton.onClick.AddListener(DestroyMail);
        mailFavoriteButton.onClick.AddListener(FavoriteMail);
    }

    public virtual void ShowMail()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideMail()
    {
        gameObject.SetActive(false);
    }

    public virtual void DestroyMail()
    {
        mailType = EEmailCategory.Remove;
        mailData.emailType = EEmailCategory.Remove;
        HideMail();
    }

    public virtual void FavoriteMail()
    {
        mailType = EEmailCategory.Favorite;
        mailData.emailType = EEmailCategory.Favorite;
    }


}
