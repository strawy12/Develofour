using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EProfileCategoryType
{
    None = 0,
    Info,
    Character,
}

[CreateAssetMenu(menuName = "SO/Profile/ProfileInfo/Category")]
public class ProfileCategoryDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfileCategory category;
    public EProfileCategoryType categoryType;
    public Sprite categorySprite;
    public string categoryName;
    [Header("Information")]
    [SerializeField]
    public List<ProfileInfoTextDataSO> infoTextList;
    public ProfileInfoTextDataSO GetSaveData(int id)
    {
        ProfileInfoTextDataSO data = null;

        foreach(ProfileInfoTextDataSO saveData in infoTextList)
        {
            if (saveData.id == id)
            {
                data = saveData;
            }
        }

        if (data == null)
        {
            Debug.LogError($"�ش� Ű: {id}�� ���� ������ ���� �ʽ��ϴ�. SO�� Ȯ���غ�����");
        }
        return data;
    }

}

