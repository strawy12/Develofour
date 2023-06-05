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

    private string sentText = "@zmail.com�� ��й�ȣ ���� ���� ������ �߼��߽��ϴ�. \nȮ�����ּ���.";

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
