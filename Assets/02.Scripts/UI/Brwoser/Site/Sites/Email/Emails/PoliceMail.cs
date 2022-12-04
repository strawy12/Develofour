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

        sr.content.anchoredPosition = new Vector2(sr.content.anchoredPosition.x, sr.content.rect.height);
    }

    public override void Init()
    {
        base.Init();

        miniGamePanel.Init();

        sendBtn.onClick.AddListener(SendMail);
    }

    public override void ShowMail()
    {

        NoticeData data = new NoticeData();
        data.head = "�������� ���� �����ϱ�";
        data.body = "�Ʒ� ���� ��ư -> ������ ��ư�� ���ʷ� Ŭ���Ͽ� ���˸� �����ϼ���.";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        base.ShowMail();
    }

    public override void HideMail()
    {
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
        SetContentPosition();
        isSendBtnClick = true;
    }


}
