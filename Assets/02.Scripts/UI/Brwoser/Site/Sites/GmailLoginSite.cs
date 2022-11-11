using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GmailLoginSite : MonoBehaviour
{
    [SerializeField]
    private string passWord;

    [SerializeField]
    private TMP_InputField gmailInputField;
    [SerializeField]
    private Button gmailLoginButton;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        gmailLoginButton.onClick?.AddListener(LoginGoogle);
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
            Debug.Log("로그인 실패");
        }
    }
}

