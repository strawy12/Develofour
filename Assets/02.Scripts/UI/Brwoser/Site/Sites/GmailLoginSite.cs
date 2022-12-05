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
    [SerializeField]
    private LoginToggle checkToggle;

    private ESiteLink requestSite;

    public override void Init()
    {
        base.Init();
        gmailInputField.asteriskChar = '·';

        gmailInputField.onSubmit?.AddListener((a) => LoginGoogle());
        gmailLoginButton.onClick?.AddListener(LoginGoogle);

        gmailInputField.onSelect.AddListener((a) => SelectInputField(true));
        gmailInputField.onDeselect.AddListener((a) => SelectInputField(false));

        gmailInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);

        showPasswordToggle.onValueChanged.AddListener(ShowPassword);

        EventManager.StartListening(ELoginSiteEvent.RequestSite, RequestSite);
    }

    private void ShowPassword(bool isShow)
    {
        isShowToggleClick = true;
 
        gmailInputField.contentType = isShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        gmailInputField.ForceLabelUpdate(); 

        checkToggle.CheckMarkEffect(isShow);
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
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginSite)
        {
            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.EnterLoginSite, 0f);

            DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginSite = true;
        }

        base.ShowSite();
    }

    private void LoginGoogle()
    {
        if (gmailInputField.text == passWord || gmailInputField.text == "11")
        {
            if (gmailInputField.text == "11")
            {
                Debug.LogError("Google Login를 Trigger를 사용하여 클리어 했습니다. 빌드 전에 해당 Trigger를 삭제하세요");
            }
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

