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
    private GameObject resetCompletedPanel;

    [SerializeField]
    private Button resetGuideNextButton;
    [SerializeField]
    private Button securityNextButton;
    [SerializeField]
    private Button passwordResetNextButton;
    [SerializeField]
    private Button resetCompletedNextButton;


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
        resetCompletedNextButton.onClick?.AddListener(CompletedPasswordReset);
    }

    // 사이트 열고 계속하기 누르면 메일가고 보안패널열림
    private void SendSecurityCodeMail()
    {
        resetGuidePanel.SetActive(false);
        securityInputPanel.SetActive(true);

        EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.StarbookSecurityCode });
    }

    // 보안패널에서 계속하기 눌러 보안코드 맞으면 비밀번호 재설정 패널 열림
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

        passwordResetPanel.SetActive(false);
        resetCompletedPanel.SetActive(true);
    }
    
    private void CompletedPasswordReset()
    {
        resetCompletedPanel.SetActive(false);
        resetGuidePanel.SetActive(true);

        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.StarbookLoginSite, 0f, false });
    }
}