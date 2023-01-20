using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscordIdentification : MonoBehaviour
{
    [Header("비밀번호")]
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
                wrongIDInputFieldText.GetComponent<TextMove>().FaliedInput("<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>");
            }

            if (passwordInputField.text != answerPassword)
            {
                currentPasswordInputFieldText.gameObject.SetActive(false);
                wrongPasswordInputFieldText.gameObject.SetActive(true);
                wrongPasswordInputFieldText.GetComponent<TextMove>().FaliedInput("<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>");
            }
        }
    }
}
