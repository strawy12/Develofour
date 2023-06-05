using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{

    public MailSaveData GetMailSaveData(int mailID)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.type == mailID);
        if (data == null)
        {
            return null;
        }
        return data;
    }

    public void SetMailSaveData(int mailID, int value)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.type == mailID);
        if (data == null)
        {
            saveData.mailSaveData.Add(new MailSaveData() { mailCategory = value, type = mailID });
            return;
        }
        DateTime dateTime = TimeSystem.TimeCount();
        data.month = dateTime.Month;
        data.day = dateTime.Day;
        data.hour = dateTime.Hour;
        data.minute = dateTime.Minute;
        data.mailCategory = value;
    }
}
