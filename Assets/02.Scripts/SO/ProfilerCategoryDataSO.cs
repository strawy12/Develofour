using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EProfilerCategoryType
{
    None = 0,
    Info,
    Character,
}

[CreateAssetMenu(menuName = "SO/Profiler/ProfilerInfo/Category")]
public class ProfilerCategoryDataSO : ScriptableObject
{
    [Header("Category")]
    public EProfilerCategory category;
    public EProfilerCategoryType categoryType;
    public Sprite categorySprite;
    public string categoryName;
    [Header("Information")]
    [SerializeField]
    public List<ProfilerInfoTextDataSO> infoTextList;
    public ProfilerInfoTextDataSO defaultInfoText;

    public ProfilerInfoTextDataSO GetSaveData(int id)
    {
        ProfilerInfoTextDataSO data = null;

        foreach(ProfilerInfoTextDataSO saveData in infoTextList)
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

