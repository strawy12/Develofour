using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    protected override void ShowSite()
    {
        NoticeData data = new NoticeData();
        data.head = "��й�ȣ ã��";
        data.body = "����â�� ���� ���� ��й�ȣ�� ã��.";

        NoticeSystem.OnGeneratedNotice?.Invoke(data);
        base.ShowSite();
    }

    private void LoginGoogle()
    {
        if (gmailInputField.text == passWord)
        {
            Debug.Log("�α��� ����");
            EventManager.TriggerEvent(EEvent.LoginGoogle);

            Browser.OnUndoSite?.Invoke();
            // ���⼭ ���� �������� �Ѿ�� �ڵ� �ֱ�
        }
        else
        {
            textMove.FaliedInput();
            Debug.Log("�α��� ����");
        }
    }

}

