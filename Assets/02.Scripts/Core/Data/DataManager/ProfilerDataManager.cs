using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public ProfilerSaveData GetProfilerSaveData(string categoryID)
    {
        if (saveData.profilerSaveData == null)
        {
            saveData.profilerSaveData = new List<ProfilerSaveData>();
        }
        ProfilerSaveData data = saveData.profilerSaveData.Find(x => x.categoryID == categoryID);

        if(data == null) 
        {
            data.categoryID = categoryID;
            data.isShowCategory = false;
            data.infoData = new List<string>();
            saveData.profilerSaveData.Add(data);
        }

        return data;
    }

    public void SaveProfilerInfoData(string categoryID, string infoID)
    {
        var infoList = GetProfilerSaveData(categoryID).infoData;
        if (infoList.Contains(infoID))
            return; 

        GetProfilerSaveData(categoryID).infoData.Add(infoID);
    }

    public void SetCategoryShow(string categoryID, bool value)
    {
        GetProfilerSaveData(categoryID).isShowCategory = value;
    }
    public bool IsCategoryShow(string categoryID)
    {
        return GetProfilerSaveData(categoryID).isShowCategory;
    }
    public bool IsProfilerInfoData(string infoID)
    {
        if (saveData.profilerSaveData == null) return false;

        foreach (var categoryData in saveData.profilerSaveData)
        {
            if (categoryData.infoData.Contains(infoID))
            {
                return true;
            }
        }
        return false;
    }

}
