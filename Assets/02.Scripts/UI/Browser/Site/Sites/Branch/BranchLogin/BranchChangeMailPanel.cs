using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchChangeMailPanel : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sentButton;
    public string answerMail;
    public BranchSentMailPanel sentPanel;

    public void Init()
    {
        sentButton.onClick.AddListener(SendMail);
    }

    public void SendMail()
    {
        if (inputField.text != answerMail) return;

        EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[2] { Constant.MailKey.BRANCH_CERTIFICATION, 0.5f });
        string frontStr = inputField.text.Substring(0, 3);
        for (int i = 0; i < inputField.text.Length - 3; i++) { frontStr += "*"; }
        sentPanel.ChangeText(frontStr);
        this.gameObject.SetActive(false);
    }
}
