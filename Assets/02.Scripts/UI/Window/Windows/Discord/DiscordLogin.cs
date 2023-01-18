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
    private TMP_InputField IDInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

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
                Debug.Log("��й�ȣ�� 11�� �Է��Ͽ� �α���");
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
                wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>�̸��� �Ǵ� ��ȭ��ȣ </b>- <i><size=85%> ��ȿ���� ���� ���̵��Դϴ�.</i>");
            }

            if(passwordInputField.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>��й�ȣ </b>- <i><size=85%> ��ȿ���� ���� ��й�ȣ�Դϴ�.</i>");
            }

        }
    }

    public void SuccessLogin()
    {
        Debug.Log("����");
        //������ �˸��� �̺�Ʈ
    }
}
