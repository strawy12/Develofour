using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void ProfileSaveData()
    {
        saveData.profileSaveData = new List<ProfileSaveData>();

        for (int i = ((int)EProfileCategory.None) + 1; i < (int)EProfileCategory.Count; i++)
        {
            saveData.profileSaveData.Add(new ProfileSaveData() { category = (EProfileCategory)i, isShowCategory = false, infoData = new List<int>() }); ;
        }
    }

    public ProfileSaveData GetProfileSaveData(EProfileCategory category)
    {
        ProfileSaveData data = saveData.profileSaveData.Find(x => x.category == category);
        return data;
    }

    public void AddProfileSaveData(EProfileCategory category, int id)
    {
        if (GetProfileSaveData(category).infoData.Contains(id))
        {
            return;
        }
        saveData.profileSaveData.Find(x => x.category == category).infoData.Add(id);
    }

    public void SetCategoryData(EProfileCategory category, bool value)
    {
        saveData.profileSaveData.Find(x => x.category == category).isShowCategory = value;
    }
    public bool IsCategoryShow(EProfileCategory category)
    {
        return saveData.profileSaveData.Find(x => x.category == category).isShowCategory;
    }
    public bool IsProfileInfoData(int id)
    {
        foreach(var categoryData in saveData.profileSaveData)
        {
            if(categoryData.infoData.Contains(id))
            {
                return true;
            }
        }
        return false;
    }
}
