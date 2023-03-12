using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProfileSaveData
{
    public bool isShow;
    public string key;
}
[Serializable]
public class ProfileCategoryData
{
    public string categoryNameText;
}

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfoData")]
public class ProfileInfoDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfileCategory category;
    public ProfileCategoryData categoryData;
    public bool isShowCategory;

    [Header("Information")]
    [SerializeField]
    public List<ProfileSaveData> saveList;
    [SerializeField]
    public List<ProfileInfoPart> partSaveList;

    public ProfileSaveData GetSaveData(string key)
    {
        ProfileSaveData data = null;

        foreach (ProfileSaveData saveData in saveList)
        {
            if (saveData.key == key)
            {
                data = saveData;
            }
        }

        if (data == null)
        {
            Debug.LogError($"해당 키: {key}에 대한 정보가 있지 않습니다. SO를 확인해보세요");
        }
        return data;
    }


    public void Reset()
    {
        foreach (var part in partSaveList)
        {
            if (part.partNameKey != "profile")
                part.isShow = false;
        }
        foreach (var data in saveList)
        {
            data.isShow = false;
        }

        isShowCategory = false;
    }
}

