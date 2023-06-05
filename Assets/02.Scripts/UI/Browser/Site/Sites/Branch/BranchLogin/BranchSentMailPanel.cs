using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchSentMailPanel : MonoBehaviour
{
    public Button mailChangeBtn;
    public BranchChangeMailPanel changePanel;
    public TMP_Text text;

    private string sentText = "@zmail.com에 비밀번호 변경 인증 메일을 발송했습니다. \n확인해주세요.";

    public void Init()
    {
        mailChangeBtn.onClick.AddListener(OpenChangePanel);
    }

    public void OpenChangePanel()
    {
        changePanel.gameObject.SetActive(true);
    }

    public void ChangeText(string getText)
    {
        text.text = getText += sentText;
    }
}
