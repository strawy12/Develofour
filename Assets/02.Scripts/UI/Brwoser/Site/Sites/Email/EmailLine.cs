using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmailLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI informationText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Button highlightedButton;

    [SerializeField]
    private Button mailButton;

    [HideInInspector]
    public EEmailCategory emailCategory;

    private Mail mail;

    public bool IsHighrighted { get { return mail.MailData.isHighlighted; } }

    public void Init(MailDataSO mailData, Mail mail)
    {
        this.mail = mail;
        ChangeText(mailData.nameText, mailData.informationText, mailData.timeText);

        mailButton.onClick.AddListener(ShowMail);
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


}
