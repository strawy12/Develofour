using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ��ȹ�̱� ��
// ���̻� ����� ���ҰŰ����ѵ� 
// isEnterBranchMail �̰� �ʿ���?
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
