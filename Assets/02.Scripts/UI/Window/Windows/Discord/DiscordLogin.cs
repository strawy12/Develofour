using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DiscordLogin : MonoBehaviour
{
    [Header("기본 로그인 정답")]
    [SerializeField]
    private string answerID;
    [SerializeField]
    private string answerPassword;

    [Header("본인 확인 로그인 정답")]
    public string identificationAnswerText;
    public string PINAnswerText;

    [SerializeField]
    private DiscordInputField IDInputField;

    [SerializeField]
    private DiscordInputField passwordInputField;

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

    public DiscordAccountPanel IDAccountPanel;
    public DiscordAccountPanel pwAccountPanel;

    public DiscordLoginBackground background;

    public DiscordIdentification identificationPanel;
    public GameObject loginPanel;

    private bool isLogin;

    public void Init()
    {
        background.OnIDPWPanelOff += SetIDPWPanel;
        identificationPanel.Init(identificationAnswerText, PINAnswerText);
        IDAccountPanel.Init();
        pwAccountPanel.Init();
        IDAccountPanel.OnClick += SetIDText;
        pwAccountPanel.OnClick += SetPWText;
        IDInputField.OnShowAccount += ShowIDAccountPanel;
        passwordInputField.OnShowAccount += ShowPWAccountPanel;
        loginButton.onClick.AddListener(OnClickLogin);
    }

    public void SetIDPWPanel()
    {
        if(!isLogin)
        {
            IDAccountPanel.gameObject.SetActive(false);
            pwAccountPanel.gameObject.SetActive(false);
        }    
    }

    public void SetIDText(string str)
    {
        IDInputField.text.text = str;
    }

    public void SetPWText(string str)
    {
        passwordInputField.text.text = str;
    }


    public void ShowIDAccountPanel()
    {
        IDAccountPanel.gameObject.SetActive(true);
    }

    public void ShowPWAccountPanel()
    {
        pwAccountPanel.gameObject.SetActive(true);
    }

    public void OnClickLogin()
    {
        if(IDInputField.text.text == answerID && passwordInputField.text.text == answerPassword)
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginSuccess);

            SuccessLogin();
        }
        else
        {
            Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.LoginFailed);
            if(IDInputField.text.text != answerID)
            {
                currentIdInputFieldText.gameObject.SetActive(false);
                wrongIDInputFieldText.gameObject.SetActive(true);
                //wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>");
            }

            if(passwordInputField.text.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                //wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>");
            }

        }
    }

    public void SuccessLogin()
    {
        //Debug.Log("성공");
        //성공을 알리는 이벤트

        //Discord Identification 켜기
        isLogin = true;
        identificationPanel.gameObject.SetActive(true);
        loginPanel.SetActive(false);
    }
}
