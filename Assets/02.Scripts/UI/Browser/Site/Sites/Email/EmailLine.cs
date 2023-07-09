using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using ExtenstionMethod;
using System;

public class EmailLine : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text titleText;

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

    public Mail mail;

    public Mail CurrentMail => mail;

    public bool IsActiveAndEnabled => mail.isActiveAndEnabled;
    public MailDataSO MailData => mail.MailData;
    public bool IsFavorited { get { return mail.MailData.isFavorited; } }

    public void Init(Mail mail)
    {
        this.mail = mail;
        ChangeText();
        mailButton.onClick.AddListener(ShowMail);
        favoriteButton.Init(MailData.isFavorited);
        favoriteButton.OnChangeFavorited += ChangeFavorite;
    }

    public void ChangeText()
    {
        if(MailData.mailCategory.ContainMask((int)EEmailCategory.Receive))
        {
            nameText.text = MailData.sendName;
        }
        else
        {
            nameText.text = $"받는 사람: {MailData.receiveName}";
        }

        titleText.text = MailData.titleText;
        informationText.text = $" - {MailData.informationText}";
        timeText.text = GetMailTimeData();
    }

    public string GetMailTimeData()
    {
        MailSaveData saveData = DataManager.Inst.GetMailSaveData(MailData.mailID);

        if(saveData != null)
        {
            return $"{saveData.month}월 {saveData.day}일";
        }
        if (MailData.Year == 2023)
        {
            return $"{MailData.Month}월 {MailData.Date}일";
        }

        else
        {
            return $"{MailData.Year.ToString().Substring(2,4)}. {MailData.Month}. {MailData.Date}.";
        }
    }

    public void ShowMail()
    {
        mail.OnOpenMail?.Invoke();
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
