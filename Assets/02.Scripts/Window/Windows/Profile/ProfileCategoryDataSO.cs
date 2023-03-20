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
    public string categoryTitle;
    [Header("Information")]
    [SerializeField]
    public List<ProfileInfoSaveData> infoTextList;

    public ProfileInfoSaveData GetSaveData(string key)
    {
        ProfileInfoSaveData data = null;

        foreach(ProfileInfoSaveData saveData in infoTextList)
        {
            if (saveData.key == key)
            {
                data = saveData;
            }
        }

        if (data == null)
        {
            Debug.LogError($"�ش� Ű: {key}�� ���� ������ ���� �ʽ��ϴ�. SO�� Ȯ���غ�����");
        }
        return data;
    }

}

