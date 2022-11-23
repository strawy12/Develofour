using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmailLine : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text informationText;

    [SerializeField]
    private TMP_Text timeText;

    [SerializeField]
    private Button favoriteButton;

    [SerializeField]
    private Button mailButton;

    [HideInInspector]
    public EEmailCategory emailCategory;

    public Mail mail;

    public bool IsFavorited { get { return mail.MailData.isHighlighted; } }

    public void Init(MailDataSO mailData, Mail mail)
    {
        this.mail = mail;
        ChangeText(mailData.nameText, mailData.informationText, mailData.timeText);
        mailButton.onClick.AddListener(ShowMail);
        favoriteButton.onClick.AddListener(AddFavorite);
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

    public void AddFavorite()
    {
        mail.FavoriteMail();
    }

}
