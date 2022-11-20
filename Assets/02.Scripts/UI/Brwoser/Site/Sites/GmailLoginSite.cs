using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

