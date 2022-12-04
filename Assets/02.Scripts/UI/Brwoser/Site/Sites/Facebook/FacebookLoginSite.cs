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
        facebookIDInputField.asteriskChar = '��';
        facebookPasswordInputField.asteriskChar = '��';
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
                
                failedLoginText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�."; 
            }
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(failedIDcnt < 3)
            {
                failedLoginText.text = "��ϵ������� �̸��� Ȥ�� ��ȭ��ȣ �Դϴ�.";
            }
            else
            {
                failedLoginText.text = "���������� Ȯ�� �� �� �ִ� ����Ʈ�� �� ������.";
            }
            failedIDcnt++;
        }
    }

    private void ClickForgetPassword()
    {
        if (facebookIDInputField.text == loginEmail)
        {
            failedLoginText.text = "��ϵ� Email�� ��й�ȣ ��������� ���½��ϴ�.";
            //email �߰�
        }
        else
        {
            failedLoginText.text = "�˸��� �̸��� Ȥ�� ��ȭ��ȣ�� �����ּ���.";
        }
    }
}
