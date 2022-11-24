using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GmailLoginSite : Site
{
    [SerializeField]
    private string passWord;
    [SerializeField]
    private Button gmailLoginButton;
    [SerializeField]
    private TMP_InputField gmailInputField;
    [SerializeField]
    private TextMove textMove;

    public override void Init()
    {
        base.Init();
        gmailLoginButton.onClick?.AddListener(LoginGoogle);

        gmailInputField.onSelect.AddListener((a) => textMove.PlaceholderEffect(true));
        gmailInputField.onDeselect.AddListener((a) => textMove.PlaceholderEffect(false));
    }

    protected override void ShowSite()
    {
        if(!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginSite)
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

            EventManager.TriggerEvent(EBrowserEvent.OnUndoSite);
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            textMove.FaliedInput();
        }
    }

}

