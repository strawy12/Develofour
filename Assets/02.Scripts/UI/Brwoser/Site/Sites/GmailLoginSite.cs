using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GmailLoginSite : Site
{
    private bool isShowToggleClick = false;

    [SerializeField]
    private string passWord;
    [SerializeField]
    private Button gmailLoginButton;
    [SerializeField]
    private TMP_InputField gmailInputField;
    [SerializeField]
    private TextMove textMove;
    [SerializeField]
    private Toggle showPasswordToggle;

    private ESiteLink requestSite;

    public override void Init()
    {
        base.Init();
        gmailInputField.asteriskChar = '·';

        gmailInputField.onSubmit?.AddListener((a) => LoginGoogle());
        gmailLoginButton.onClick?.AddListener(LoginGoogle);

        gmailInputField.onSelect.AddListener((a) => SelectInputField(true));
        gmailInputField.onDeselect.AddListener((a) => SelectInputField(false));

        showPasswordToggle.onValueChanged.AddListener(ShowPassword);

        EventManager.StartListening(ELoginSiteEvent.RequestSite, RequestSite);
    }

    private void ShowPassword(bool isShow)
    {
        isShowToggleClick = true;

        gmailInputField.contentType = isShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        gmailInputField.ActivateInputField();
    }

    private void Update()
    {
        if(gmailInputField.isFocused)
        {
           Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }

    private void SelectInputField(bool isSelected)
    {
        if(isShowToggleClick)
        {
            isShowToggleClick = false;
            return;
        }

        Input.imeCompositionMode = isSelected ? IMECompositionMode.Off : IMECompositionMode.Auto;
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
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginSite)
        {
            NoticeData data = new NoticeData();
            data.head = "비밀번호 찾기";
            data.body = "메일창을 들어가기 위해 비밀번호를 찾기.";

            NoticeSystem.OnGeneratedNotice?.Invoke(data);

            DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginSite = true;
        }

        base.ShowSite();
    }

    private void LoginGoogle()
    {
        if (gmailInputField.text == passWord)
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);
            EventManager.TriggerEvent(ELoginSiteEvent.LoginSuccess);

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
            textMove.FaliedInput();
        }
    }

}

