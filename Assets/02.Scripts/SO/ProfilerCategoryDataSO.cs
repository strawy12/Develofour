using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EProfilerCategoryType
{
    None = 0,
    Info,
    Character,
    Visiable,
}

[CreateAssetMenu(menuName = "SO/Profiler/ProfilerInfo/Category")]
public class ProfilerCategoryDataSO : ScriptableObject
{
    [Header("Category")]
    private string id;
    public string ID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(value))
                return;
            id= value;
        }
    }

    public EProfilerCategoryType categoryType;
    
    public Sprite categorySprite;
    public string categoryName;

    [Header("Information")]
    [SerializeField]
    public List<string> infoIDList;
    public string defaultInfoID;
}

