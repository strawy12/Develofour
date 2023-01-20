using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscordIdentification : MonoBehaviour
{
    [Header("��й�ȣ")]
    public string identificationAnswerText;
    public string PINAnswerText;

    public DiscordHideAndShow identificationHidePanel;
    public DiscordHideAndShow PINHidePanel;
    public TMP_InputField identificationInputfield;
    public TMP_InputField PINInputfield;

    public void OnClickSubmisstion()
    {
        if (identificationInputfield.text == identificationAnswerText && identificationInputfield.text == PINAnswerText
            || identificationInputfield.text == "11" && PINInputfield.text == "11")
        {
            this.gameObject.SetActive(false);   
        }
        else
        {
            //
            if (identificationInputfield.text != identificationAnswerText)
            {
                currentIdInputFieldText.gameObject.SetActive(false);
                wrongIDInputFieldText.gameObject.SetActive(true);
                wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>�̸��� �Ǵ� ��ȭ��ȣ </b>- <i><size=85%> ��ȿ���� ���� ���̵��Դϴ�.</i>");
            }

            if (passwordInputField.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>��й�ȣ </b>- <i><size=85%> ��ȿ���� ���� ��й�ȣ�Դϴ�.</i>");
            }
        }
    }
}
