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
        NoticeData data = new NoticeData();
        data.head = "비밀번호 찾기";
        data.body = "메일창을 들어가기 위해 비밀번호를 찾기.";

        NoticeSystem.OnGeneratedNotice?.Invoke(data);
        base.ShowSite();
    }

    private void LoginGoogle()
    {
        if (gmailInputField.text == passWord)
        {
            Debug.Log("로그인 성공");
            EventManager.TriggerEvent(EEvent.LoginGoogle);

            Browser.OnUndoSite?.Invoke();
            // 여기서 다음 페이지로 넘어가는 코드 넣기
        }
        else
        {
            textMove.FaliedInput();
            Debug.Log("로그인 실패");
        }
    }

}

