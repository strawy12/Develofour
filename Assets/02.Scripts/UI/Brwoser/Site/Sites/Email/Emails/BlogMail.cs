using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlogMail : Mail
{
    public override void ShowMail()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.OpenBrunchDeleteMail, 0f);

        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterBranchMail)
        {
            EventManager.TriggerEvent(EQuestEvent.ShowBrunchGmail);
            DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterBranchMail = true;
        }

        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }
}
