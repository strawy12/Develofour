using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class EmailLine : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text informationText;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private EmailFavoriteButton favoriteButton;

    [SerializeField]
    private Button mailButton;

    public int Category
    {
        get
        {
            return mail.MailData.mailCategory;
        }

        set
        {
            mail.MailData.mailCategory = value;
        }
    }

    private Mail mail;

    public Mail CurrentMail => mail;

    public bool IsActiveAndEnabled => mail.isActiveAndEnabled;
    public MailDataSO MailData => mail.MailData;
    public bool IsFavorited { get { return mail.MailData.isFavorited; } }

    public void Init(MailDataSO mailData, Mail mail)
    {
        this.mail = mail;
        ChangeText(mailData.Name, mailData.Info, mailData.Time);
        mailButton.onClick.AddListener(ShowMail);
        favoriteButton.Init(mailData.isFavorited);
        favoriteButton.OnChangeFavorited += ChangeFavorite;
    }

    public void ChangeText(string name, string info, string time)
    {
        nameText.text = name;
        informationText.text = info;
        timeText.text = time;
    }

    public void ShowMail()
    {
        mail.ShowMail();
    }

    public void HideMail()
    {
        mail.HideMail();
    }

    public void ChangeFavorite(bool isFavorited)
    {
        mail.FavoriteMail(isFavorited);
    }

    public void ChangeRemoveCategory()
    {
        favoriteButton.ImmediatellyStop();
    }

}
