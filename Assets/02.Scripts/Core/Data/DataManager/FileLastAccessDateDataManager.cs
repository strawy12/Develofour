using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager
{
    public void AddLastAccessDateData(int id,DateTime dateTime)
    {
        LastAccessDateData data =  SaveData.lastAccessDateData.Find(x=> x.fileID == id);
        string dateData = $"{dateTime.Year}년 {dateTime.Month.ToString("D2")}월 {dateTime.Day.ToString("D2")}일";

        if(data == null)
        {
            data = new LastAccessDateData() { fileID = id, date = dateData };
            SaveData.lastAccessDateData.Add(data);
        }

        data.date = dateData;
    }

    public string GetLastAcccestDate(int id)
    {
        LastAccessDateData data = SaveData.lastAccessDateData.Find(x => x.fileID == id);
        if(data == null)
        {
            return "";
        }
        return data.date;
    }
}
