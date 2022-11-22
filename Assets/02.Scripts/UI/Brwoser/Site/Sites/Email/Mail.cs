using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mail : MonoBehaviour
{

    [SerializeField]
    protected EEmailCategory mailType;

    protected MailDataSO mailData;

    public MailDataSO MailData { get { return mailData; } }

    public virtual void Init(MailDataSO mailData)
    {
        this.mailData = mailData;
    }

    public virtual void ShowMail()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideMail()
    {
        gameObject.SetActive(false);
    }
}
