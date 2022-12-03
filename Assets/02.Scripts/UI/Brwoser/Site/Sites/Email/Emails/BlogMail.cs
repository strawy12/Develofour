using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlogMail : Mail
{
    public override void ShowMail()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeDataType.Blog);

        EventManager.TriggerEvent(EQuestEvent.BlogCleanUp);

        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }
}
