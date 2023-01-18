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
        passwordField.SetPassword(password);

        gmailLoginButton.onClick?.AddListener(passwordField.TryLogin);

        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;

        passwordField.onSelect.AddListener((a) => SelectInputField(true));
        passwordField.onDeselect.AddListener((a) => SelectInputField(false));

        passwordToggle.onValueChanged.AddListener(ShowPassword);

        EventManager.StartListening(ELoginSiteEvent.RequestSite, RequestSite);
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

    private void RequestSite(object[] ps)
    {
        if (!(ps[0] is ESiteLink)) { return; }
        if (requestSite != ESiteLink.None) { return; }
        requestSite = (ESiteLink)ps[0];
    }

    protected override void ShowSite()
    {
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite)
        {
            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.EnterLoginSite, 0f);
        }

        base.ShowSite();
    }

    private void SuccessLogin()
    {
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
        EventManager.TriggerEvent(ELoginSiteEvent.LoginSuccess);

        DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite = true;

        if (requestSite == ESiteLink.None)
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
            EventManager.TriggerEvent(ELoginSiteEvent.LoginSuccess);

            DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite = true;

            if (requestSite == ESiteLink.None)
            {
                EventManager.TriggerEvent(EBrowserEvent.OnUndoSite);
            }
            else
            {
                EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY , false});
                requestSite = ESiteLink.None;
            }
            EventManager.TriggerEvent(EBrowserEvent.OnUndoSite);
        }
        else
        {
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY });
            requestSite = ESiteLink.None;
        }
    }

    private void FailLogin()
    {
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
        textMove.FaliedInput();
    }

}

