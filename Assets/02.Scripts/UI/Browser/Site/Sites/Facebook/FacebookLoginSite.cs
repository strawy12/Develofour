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
    private string password;
    [SerializeField]
    private Button LoginBtn;
    [SerializeField]
    private TMP_InputField facebookIDInputField;
    [SerializeField]
    private PasswordInputField passwordField;
    [SerializeField]
    private Button forgetPasswordBtn;
    [SerializeField]
    private TMP_Text failedLoginText;

    private ESiteLink requestSite;

    private int failedIDcnt = 0;
    public override void Init()
    {
        base.Init();
        passwordField.SetPassword(password);

        failedIDcnt = 0;

        failedLoginText.text = "";
        failedLoginText.color = Color.black;

        facebookIDInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);
        LoginBtn.onClick.AddListener(LoginFacebook);

        passwordField.OnSuccessLogin += LoginSuccess;
        passwordField.OnFailLogin += LoginFail;

        forgetPasswordBtn?.onClick.AddListener(ClickForgetPassword);

        EventManager.StartListening(ELoginSiteEvent.FacebookRequestSite, RequestSite);
    }

    private void RequestSite(object[] ps)
    {
        if (!(ps[0] is ESiteLink)) { return; }
        if (requestSite != ESiteLink.None) { return; }
        requestSite = (ESiteLink)ps[0];

        failedLoginText.color = Color.black;
    }

    private void LoginSuccess()
    {
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
        EventManager.TriggerEvent(ELoginSiteEvent.FacebookLoignSuccess);

        Debug.Log(requestSite);
        if (requestSite == ESiteLink.None)
        {
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.Facebook, Constant.LOADING_DELAY });
        }
        else
        {
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY });
            requestSite = ESiteLink.None;
        }

        // TODO
        // ���� ���� �ؾ���
        DataManager.Inst.CurrentPlayer.CurrentChapterData.isLoginSNSSite = true;
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.WriterFacebookLoginSuccess, 0f);
    }

    private void LoginFail()
    {
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);

        failedLoginText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
        failedLoginText.color = Color.red;
    }
    private void LoginFacebook()
    {
        failedLoginText.text = "";
        if (facebookIDInputField.text == loginEmail)
        {
            passwordField.TryLogin();
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(failedIDcnt < 3)
            {
                failedLoginText.text = "��ϵ������� �̸��� Ȥ�� ��ȭ��ȣ �Դϴ�.";
                failedLoginText.color = Color.red;
            }
            else
            {
                failedLoginText.text = "���������� Ȯ�� �� �� �ִ� ����Ʈ�� �� ������.";
                failedLoginText.color = Color.black;
            }
            failedIDcnt++;
        }
    }

    private void ClickForgetPassword()
    {
        if (facebookIDInputField.text == loginEmail)
        {
            failedLoginText.text = "��ϵ� Email�� ��й�ȣ ��������� ���½��ϴ�.";
            failedLoginText.color = Color.black; 
            EventManager.StartListening(ELoginSiteEvent.FacebookNewPassword, NewPassword);
        
            EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.SnsPasswordChange});

            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.SnsSetNewPassword, 0f);

        }
        else
        {
            failedLoginText.text = "�˸��� �̸��� Ȥ�� ��ȭ��ȣ�� �����ּ���.";
            failedLoginText.color = Color.red;
        }
    }

    private void NewPassword(object[] param)
    {
        if(param == null || !(param[0] is string)) { return; }

        password = param[0] as string;
        DataManager.Inst.CurrentPlayer.CurrentChapterData.snsPassword = password;
        passwordField.SetPassword(password);

        EventManager.StopListening(ELoginSiteEvent.FacebookNewPassword, NewPassword);
    }
}
