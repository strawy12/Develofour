using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [SerializeField]
    private ScrollRect sr;

    public async void SetContentPosition()
    {
        await Task.Delay(200);

        sr.verticalNormalizedPosition = 0;
    }

    public override void Init()
    {
        base.Init();

        miniGamePanel.Init();

        sendBtn.onClick.AddListener(SendMail);
    }

    public override void ShowMail()
    {
        if (!DataManager.Inst.CurrentPlayer.questClearData.isPoliceMiniGameClear)
        {
            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.OpenPoliceMail, 0f);
        }

        base.ShowMail();
    }

    public override void HideMail()
    {
        if(miniGamePanel.IsStarted)
        {
            sendButtonPanel.SetActive(true);
            miniGamePanel.InitializationGame();
        }

        base.HideMail();
    }

    public override void DestroyMail()
    {
        if (!miniGamePanel.IsCleared) return;

        base.DestroyMail();
    }

    public void SendMail()
    {
        sendButtonPanel.SetActive(false);
        miniGamePanel.gameObject.SetActive(true);
        miniGamePanel.OnGameClear += MiniGameClear;

        SetContentPosition();
        
        isSendBtnClick = true;
    }

    private void MiniGameClear()
    {
        miniGamePanel.OnGameClear -= MiniGameClear;
        miniGamePanel.gameObject.SetActive(false);
        sendButtonPanel.SetActive(true);

        SetContentPosition();

        isSendBtnClick = false;
    }
}
