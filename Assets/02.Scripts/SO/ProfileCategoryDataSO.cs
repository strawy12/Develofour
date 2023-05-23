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
            Debug.LogError($"해당 키: {id}에 대한 정보가 있지 않습니다. SO를 확인해보세요");
        }
        return data;
    }

}

