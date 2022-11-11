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
    private TMP_Text gamilText;
    [SerializeField]
    private Button gmailLoginButton;
    [SerializeField]
    private TMP_InputField gmailInputField;
    [SerializeField]
    private TextMove textMove;

    void Awake()
    {
        Init();
    }

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
            // 여기서 다음 페이지로 넘어가는 코드 넣기
        }
        else
        {
            Color color = new Color(255, 0, 0);
            gamilText.text = "다시 입력하세요.";
            gamilText.DOColor(color, 0.2f);

            Debug.Log("로그인 실패");
        }
    }

}

