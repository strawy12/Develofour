using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceMail : Mail
{
    [SerializeField]
    private PoliceMiniGame miniGamePanel;

    [SerializeField]
    private GameObject sendButtonPanel;

    [SerializeField]
    private Button sendBtn;

    private bool isSendBtnClick;
    private bool isFinish = false;

    public override void Init()
    {
        base.Init();
        sendBtn.onClick.AddListener(SendMail);
    }

    public override void ShowMail()
    {
        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }

    public override void DestroyMail()
    {
        if (isFinish) return;

        base.DestroyMail();
    }

    public void SendMail()
    {
        sendButtonPanel.SetActive(false);
        miniGamePanel.gameObject.SetActive(true);

        isSendBtnClick = true;
    }
}
