using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DiscordLogin : MonoBehaviour
{
    [Header("정답")]
    [SerializeField]
    private string answerID;
    [SerializeField]
    private string answerPassword;

    [SerializeField]
    private TMP_InputField IDInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

    [SerializeField]
    private Button loginButton;

    [Header("오답텍스트")]
    [SerializeField]
    private TextMeshProUGUI wrongIDInputFieldText;
    [SerializeField]
    private TextMeshProUGUI wrongPasswordInputFieldText;

    [Header("기본텍스트")]
    [SerializeField]
    private TextMeshProUGUI currentIdInputFieldText;
    [SerializeField]
    private TextMeshProUGUI currentPasswordInputFieldText;

    public void Init()
    {
        loginButton.onClick.AddListener(OnClickLogin);
        IDInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);
        passwordInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);
    }

    public void OnClickLogin()
    {
        if(IDInputField.text == answerID && passwordInputField.text == answerPassword
            || IDInputField.text == "11" && passwordInputField.text == "11")
        {
            if(IDInputField.text == "11")
            {
                Debug.Log("비밀번호를 11로 입력하여 로그인");
            }

            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);

            SuccessLogin();
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(IDInputField.text != answerID)
            {
                currentIdInputFieldText.gameObject.SetActive(false);
                wrongIDInputFieldText.gameObject.SetActive(true);
                wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>");
            }

            if(passwordInputField.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>");
            }

        }
    }

    public void SuccessLogin()
    {
        Debug.Log("성공");
        //성공을 알리는 이벤트
    }
}
