using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscordIdentification : MonoBehaviour
{
    [Header("��й�ȣ")]
    public string identificationAnswerText;
    public string PINAnswerText;

    //public DiscordHideAndShow identificationHidePanel;
    public DiscordHideAndShow PINHidePanel;
    public TMP_InputField identificationInputfield;
    public TextMove identificationTextmove;
    public TMP_InputField PINInputfield;
    public TextMove PINTextmove;

    public GameObject loginPanel;

    public Button loginBtn;

    void Start()
    {
        Debug.Log("DiscordIdentification ��ũ��Ʈ 11 ����� �ڵ� �����");
    }

    public void Init()
    {
        loginBtn.onClick.AddListener(OnClickSubmisstion);
    }

    public void OnClickSubmisstion()
    {
        if (identificationInputfield.text == identificationAnswerText && identificationInputfield.text == PINAnswerText
            || identificationInputfield.text == "11" && PINInputfield.text == "11")
        {
            if(identificationInputfield.text == "11")
            {
                Debug.Log("DiscordIdentification ��ũ��Ʈ 11 ����� �ڵ� ");
            }
            loginPanel.SetActive(false);
        }
        else
        {
            //
            if (identificationInputfield.text != identificationAnswerText)
            {
                identificationInputfield.gameObject.SetActive(true);
                //identificationTextmove.FaliedInput("<b>�̸��� �Ǵ� ��ȭ��ȣ </b>- <i><size=85%> ��ȿ���� ���� ���̵��Դϴ�.</i>");
            }

            if (PINInputfield.text != PINAnswerText)
            {
                PINInputfield.gameObject.SetActive(true);
                //PINTextmove.FaliedInput("<b>��й�ȣ </b>- <i><size=85%> ��ȿ���� ���� ��й�ȣ�Դϴ�.</i>");
            }
        }
    }
}
