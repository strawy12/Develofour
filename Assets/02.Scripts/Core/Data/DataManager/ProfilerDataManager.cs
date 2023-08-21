using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void ProfilerSaveData()
    {
        saveData.profilerSaveData = new List<ProfilerSaveData>();

        for (int i = ((int)EProfilerCategory.None) + 1; i < (int)EProfilerCategory.Count; i++)
        {
            saveData.profilerSaveData.Add(new ProfilerSaveData() { category = (EProfilerCategory)i, isShowCategory = false, infoData = new List<int>() }); ;
        }
    }

    public ProfilerSaveData GetProfilerSaveData(EProfilerCategory category)
    {
        ProfilerSaveData data = saveData.profilerSaveData.Find(x => x.category == category);
        return data;
    }

    public void AddProfilerSaveData(EProfilerCategory category, int id)
    {
        if (GetProfilerSaveData(category).infoData.Contains(id))
        {
            return;
        }
        saveData.profilerSaveData.Find(x => x.category == category).infoData.Add(id);
    }

    public void SetCategoryData(EProfilerCategory category, bool value)
    {
        saveData.profilerSaveData.Find(x => x.category == category).isShowCategory = value;
    }
    public bool IsCategoryShow(EProfilerCategory category)
    {
        return saveData.profilerSaveData.Find(x => x.category == category).isShowCategory;
    }
    public bool IsProfilerInfoData(int id)
    {
        Debug.Log(id);
        foreach(var categoryData in saveData.profilerSaveData)
        {
            if(categoryData.infoData.Contains(id))
            {
                return true;
            }
        }
        return false;
    }
}
