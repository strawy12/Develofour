using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StarbookPasswordResetSite : Site
{
    [SerializeField]
    private string securityAnswer;

    [SerializeField]
    private GameObject resetGuidePanel;
    [SerializeField]
    private GameObject securityInputPanel;
    [SerializeField]
    private GameObject passwordResetPanel;

    [SerializeField]
    private Button resetGuideNextButton;
    [SerializeField]
    private Button securityNextButton;
    [SerializeField]
    private Button passwordResetNextButton;


    [SerializeField]
    private TMP_InputField securityInput;
    [SerializeField]
    private TMP_InputField passwordResetInput;


    public override void Init()
    {
        base.Init();

        resetGuideNextButton.onClick?.AddListener(SendSecurityCodeMail);
        securityNextButton.onClick?.AddListener(CheckSecurityCodeAnswer);
        passwordResetNextButton.onClick?.AddListener(PasswordResetFinish);
    }

    // ����Ʈ ���� ����ϱ� ������ ���ϰ��� �����гο���
    private void SendSecurityCodeMail()
    {
        resetGuidePanel.SetActive(false);
        securityInputPanel.SetActive(true);

        EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.SnsPasswordChange });
    }

    // �����гο��� ����ϱ� ���� �����ڵ� ������ ��й�ȣ �缳�� �г� ����
    private void CheckSecurityCodeAnswer()
    {
        if(securityInput.text == securityAnswer)
        {
            securityInputPanel.SetActive(false);
            passwordResetPanel.SetActive(true);  
        }
        else
        {
            securityInput.text = "";
        }
    }

    private void PasswordResetFinish()
    {
        EventManager.TriggerEvent(ELoginSiteEvent.FacebookNewPassword, new object[] { passwordResetInput.text });
    }

}