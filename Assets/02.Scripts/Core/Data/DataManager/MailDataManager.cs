using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void CreateMailData()
    {
        if(saveData.mailSaveData.Count == 0)
        {
            foreach(var mail in ResourceManager.Inst.MailDataSOList)
            {
                saveData.mailSaveData.Add(new MailSaveData() { mailCategory = mail.Value.mailCategory, type = mail.Value.Type });
            }

        }
    }

    public MailSaveData GetMailSaveData(EMailType mailType)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.type == mailType);
        if (data == null)
        {
            Debug.Log("없는 메일 데이터 입니다.");
        }
        return data;
    }

    //public bool Check

    public void SetMailSaveData(EMailType mailType, int value)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.type == mailType);
        if (data == null)
        {
            saveData.mailSaveData.Add(new MailSaveData() { mailCategory = value, type = mailType });
            return;
        }
        data.mailCategory = value;
    }
}
