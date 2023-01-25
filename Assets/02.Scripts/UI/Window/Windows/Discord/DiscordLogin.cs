using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DiscordLogin : MonoBehaviour
{
    [Header("����")]
    [SerializeField]
    private string answerID;
    [SerializeField]
    private string answerPassword;

    [SerializeField]
    private DiscordInputField IDInputField;

    [SerializeField]
    private DiscordInputField passwordInputField;

    [SerializeField]
    private Button loginButton;

    [Header("�����ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI wrongIDInputFieldText;
    [SerializeField]
    private TextMeshProUGUI wrongPasswordInputFieldText;

    [Header("�⺻�ؽ�Ʈ")]
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
        identificationPanel.Init();
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
                //wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>�̸��� �Ǵ� ��ȭ��ȣ </b>- <i><size=85%> ��ȿ���� ���� ���̵��Դϴ�.</i>");
            }

            if(passwordInputField.text.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                //wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>��й�ȣ </b>- <i><size=85%> ��ȿ���� ���� ��й�ȣ�Դϴ�.</i>");
            }

        }
    }

    public void SuccessLogin()
    {
        //Debug.Log("����");
        //������ �˸��� �̺�Ʈ

        //Discord Identification �ѱ�
        isLogin = true;
        identificationPanel.gameObject.SetActive(true);
        loginPanel.SetActive(false);
    }
}
