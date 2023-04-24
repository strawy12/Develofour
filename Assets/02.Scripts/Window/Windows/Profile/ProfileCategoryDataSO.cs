using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EProfileCategoryType
{
    Scene,
    Character,
}

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfo/Category")]
public class ProfileCategoryDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfileCategory category;
    public EProfileCategoryType categoryType;
    [Header("Information")]
    [SerializeField]
    public List<ProfileInfoTextDataSO> infoTextList;

    public ProfileInfoTextDataSO GetSaveData(string key)
    {
        ProfileInfoTextDataSO data = null;

        foreach(ProfileInfoTextDataSO saveData in infoTextList)
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

