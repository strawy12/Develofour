using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GmailLoginSite : Site 
{
    private bool isShowToggleClick = false;

    [SerializeField]
    private string password;
    [SerializeField]
    private Button gmailLoginButton;
    [SerializeField]
    private PasswordInputField passwordField;
    [SerializeField]
    private TextMove textMove;
    [SerializeField]
    private PasswordToggle passwordToggle;

    private ESiteLink requestSite;

    public override void Init()
    {
        base.Init();
        passwordField.InputField.asteriskChar = '·';
        passwordField.SetPassword(password);

        gmailLoginButton.onClick?.AddListener(passwordField.TryLogin);

        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;

        passwordField.onSelect.AddListener((a) => SelectInputField(true));
        passwordField.onDeselect.AddListener((a) => SelectInputField(false));

        passwordToggle.onValueChanged.AddListener(ShowPassword);

        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: GmailLoginButtonClick);
    }

    private void GmailLoginButtonClick()
    {
        gmailLoginButton.onClick?.Invoke();
    }

    private void ShowPassword(bool isShow)
    {
        isShowToggleClick = true;
 
        passwordField.InputField.contentType = isShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        passwordField.InputField.ForceLabelUpdate();
    }

    private void SelectInputField(bool isSelected)
    {
        textMove.PlaceholderEffect(isSelected);
    }

    protected override void ShowSite()
    {
        base.ShowSite();
    }

    private void SuccessLogin()
    {
       // Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
        EventManager.TriggerEvent(ELoginSiteEvent.LoginSuccess);

        if (requestSite == ESiteLink.None)
        {
            EventManager.TriggerEvent(ELoginSiteEvent.LoginSuccess);

            InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: GmailLoginButtonClick);
        }
        else
        {
          //  Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            textMove.FaliedInput("다시 입력하세요");
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY });
            requestSite = ESiteLink.None;
        }
    }

    private void FailLogin()
    {
       // Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
        textMove.FaliedInput("틀린 비밀번호입니다");
    }

}

