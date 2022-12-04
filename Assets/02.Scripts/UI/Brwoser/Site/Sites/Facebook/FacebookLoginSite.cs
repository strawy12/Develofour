using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class FacebookLoginSite : Site
{

    [SerializeField]
    private string loginEmail;
    [SerializeField]
    private string passWord;
    [SerializeField]
    private Button LoginBtn;
    [SerializeField]
    private TMP_InputField facebookIDInputField;
    [SerializeField]
    private TMP_InputField facebookPasswordInputField;
    [SerializeField]
    private Button forgetPasswordBtn;
    [SerializeField]
    private TMP_Text failedLoginText;

    private ESiteLink requestSite;

    private int failedIDcnt = 0;
    public override void Init()
    {
        base.Init();
        failedIDcnt = 0;
        facebookIDInputField.asteriskChar = '·';
        facebookPasswordInputField.asteriskChar = '·';
        failedLoginText.text = "";
        LoginBtn.onClick.AddListener(LoginFacebook);
        facebookIDInputField.onSelect.AddListener((a) => SelectInputField(true));
        facebookIDInputField.onDeselect.AddListener((a) => SelectInputField(false));
        facebookPasswordInputField.onSelect.AddListener((a) => SelectInputField(true));
        facebookPasswordInputField.onSelect.AddListener((a) => SelectInputField(false));

        forgetPasswordBtn?.onClick.AddListener(ClickForgetPassword);

        facebookPasswordInputField.contentType = TMP_InputField.ContentType.Password;
        facebookPasswordInputField.ActivateInputField();

        EventManager.StartListening(ELoginSiteEvent.EmailRequestSite, RequestSite);
    }
    private void RequestSite(object[] ps)
    {
        if (!(ps[0] is ESiteLink)) { return; }
        if (requestSite != ESiteLink.None) { return; }
        requestSite = (ESiteLink)ps[0];
    }
    private void SelectInputField(bool isSelected)
    {
        Input.imeCompositionMode = isSelected ? IMECompositionMode.Off : IMECompositionMode.Auto;
    }

    private void Update()
    {
        if(facebookPasswordInputField.isFocused || facebookIDInputField.isFocused)
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }

    private void LoginFacebook()
    {
        if (facebookIDInputField.text == loginEmail)
        { 
            if(facebookPasswordInputField.text == passWord)
            {
                Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
                EventManager.TriggerEvent(ELoginSiteEvent.FacebookLoignSuccess);

                if (requestSite == ESiteLink.None)
                {
                    EventManager.TriggerEvent(EBrowserEvent.OnUndoSite);
                }
                else
                {
                    EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY });
                    requestSite = ESiteLink.None;
                }
            }
            else
            {
                Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
                
                failedLoginText.text = "비밀번호가 일치하지 않습니다."; 
            }
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(failedIDcnt < 3)
            {
                failedLoginText.text = "등록되지않은 이메일 혹은 전화번호 입니다.";
            }
            else
            {
                failedLoginText.text = "개인정보를 확인 할 수 있는 사이트를 들어가 보세요.";
            }
            failedIDcnt++;
        }
    }

    private void ClickForgetPassword()
    {
        if (facebookIDInputField.text == loginEmail)
        {
            failedLoginText.text = "등록된 Email에 비밀번호 변경메일을 보냈습니다.";
            //email 추가
        }
        else
        {
            failedLoginText.text = "알맞은 이메일 혹은 전화번호를 적어주세요.";
        }
    }
}
