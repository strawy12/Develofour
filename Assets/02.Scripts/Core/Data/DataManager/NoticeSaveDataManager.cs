using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateNoticeDataSave()
    {
        saveData.saveNoticeData = new List<NoticeData>();
    }
    public void AddNoticeData(NoticeData data)
    {
        if(!saveData.saveNoticeData.Contains(data)) {
            saveData.saveNoticeData.Add(data);
        }
    }
    public void SetNoticeDataSave(List<NoticeData> noticeDatas)
    {
        saveData.saveNoticeData = noticeDatas;
    }

}
