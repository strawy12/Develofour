using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 옛날 기획이긴 해
// 더이상 사용을 안할거같긴한데 
// isEnterBranchMail 이게 필요함?
// 
public class BlogMail : Mail
{
    public override void ShowMail()
    {
        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.OpenBrunchDeleteMail, 0f);

        if (!DataManager.GetSaveData<bool>(ESaveDataType.IsWindowsLoginAdminMode))
        {
            //EventManager.TriggerEvent(EQuestEvent.ShowBrunchGmail);
            //DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterBranchMail = true;
        }

        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }
}
