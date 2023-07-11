using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{

    public MailSaveData GetMailSaveData(string mailID)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.id == mailID);
        if (data == null)
        {
            return null;
        }
        return data;
    }

    public void SetMailSaveData(string mailID)
    {
        MailSaveData data = saveData.mailSaveData.Find(x => x.id == mailID);
        if (data == null)
        {
            saveData.mailSaveData.Add(new MailSaveData() { id = mailID });
            return;
        }
        DateTime dateTime = TimeSystem.TimeCount();
        data.month = dateTime.Month;
        data.day = dateTime.Day;
        data.hour = dateTime.Hour;
        data.minute = dateTime.Minute;
    }
}
