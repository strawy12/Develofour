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

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfoData")]
public class ProfileInfoDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfileCategory category;
    public bool isShowCategory;

    [Header("Information")]
    [SerializeField]
    public List<ProfileSaveData> saveList;

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
        foreach (var data in saveList)
        {
            data.isShow = false;
        }

        isShowCategory = false;
    }
}

