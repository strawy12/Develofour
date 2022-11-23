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
    private Button mailHighlightedButton;
    #endregion
    [SerializeField]
    protected EEmailCategory mailType;

    [SerializeField]
    protected MailDataSO mailData;

    public MailDataSO MailData { get { return mailData; } }

    public virtual void Init(MailDataSO mailData)
    {
        this.mailData = mailData;
        mailCloseButton.onClick.AddListener(HideMail);
        mailDestroyButton.onClick.AddListener(DestroyMail);
        mailHighlightedButton.onClick.AddListener(HighlightedMail);
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

    public virtual void HighlightedMail()
    {
        mailType = EEmailCategory.Highlighted;
        mailData.emailType = EEmailCategory.Highlighted;
    }
}
