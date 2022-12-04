using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FacebookNewPasswordMail : Mail
{
    [SerializeField]
    private Button CompleteBtn;
    [SerializeField]
    private TMP_InputField inputPassword;
    [SerializeField]
    private int minTextCnt = 5;
    [SerializeField]
    private int maxTextCnt = 12;
    [SerializeField]
    private TMP_Text failedText;
    [SerializeField]
    private GameObject newPasswordPanel;
    [SerializeField]
    private GameObject completeMassagePanel;
    public override void Init()
    {
        base.Init();
        failedText.text = "";
        completeMassagePanel.SetActive(true);
        completeMassagePanel.SetActive(false);
        CompleteBtn.onClick.AddListener(MakeNewPassword);
    }

    private void Update()
    {
        if (inputPassword.isFocused)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }

    private void MakeNewPassword()
    {
        if(inputPassword.text.Length < minTextCnt || inputPassword.text.Length > maxTextCnt)
        {
            failedText.text = $"비밀번호는 최소 {minTextCnt}자리 ~ 최대 {maxTextCnt}자리입니다.";
            return;
        }

        newPasswordPanel.SetActive(false);
        completeMassagePanel.SetActive(true);
        EventManager.TriggerEvent(ELoginSiteEvent.FacebookNewPassword, new object[] { inputPassword.text });

    }
    
}
