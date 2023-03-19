using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProfileInfoSaveData
{
    public string key;
}

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfo/Data")]
public class ProfileCategoryDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfileCategory category;
    [Header("Information")]
    [SerializeField]
    public List<ProfileInfoSaveData> saveList;

    public ProfileInfoSaveData GetSaveData(string key)
    {
        ProfileInfoSaveData data = null;

        foreach(ProfileInfoSaveData saveData in saveList)
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

}

