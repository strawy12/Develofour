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

    [SerializeField]
    private ScrollRect sr;

    public void SetContentPosition()
    {
        sr.content.localPosition = new Vector3(0, 450, 0);
    }

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
        Invoke("SetContentPosition", 0.2f);
        isSendBtnClick = true;
    }


}
