using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mail : MonoBehaviour
{
    #region 버튼들
    [SerializeField]
    private Button mailCloseButton;

    [SerializeField]
    private Button mailDestroyButton;

    [SerializeField]
    private EmailFavoriteButton mailFavoriteButton;
    #endregion
    [SerializeField]
    protected EEmailCategory mailType;

    [SerializeField]
    protected MailDataSO mailData;

    public MailDataSO MailData { get { return mailData; } }

    public Action OnChangeCatagory;

    [SerializeField]
    private bool isCanDelete = true;

    public virtual void Init()
    {
        mailCloseButton.onClick.AddListener(HideMail);
        mailDestroyButton.onClick.AddListener(DestroyMail);
        mailFavoriteButton.Init(mailData.isHighlighted);
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
        //즐겨찾기 연출 해제
        if(mailType == EEmailCategory.Favorite)
        {
            mailFavoriteButton.OnChangeMailType?.Invoke();
        }

        mailType = EEmailCategory.Remove;
        mailData.emailType = EEmailCategory.Remove;
        OnChangeCatagory?.Invoke();
        HideMail();
    }

    public virtual void FavoriteMail()
    {
        Debug.Log("사용, 현재 mailData.emailType = " + MailData.emailType + "  현재 mailType = " + mailType );
        if(mailType != EEmailCategory.Receive)
        {
            mailType = EEmailCategory.Receive;
            mailData.emailType = EEmailCategory.Receive;
        }    
        else
        {
            mailType = EEmailCategory.Favorite;
            mailData.emailType = EEmailCategory.Favorite;
        }
        Debug.Log("끝 , 현재 mailData.emailType = " + MailData.emailType + "  현재 mailType = " + mailType);
    }


}
