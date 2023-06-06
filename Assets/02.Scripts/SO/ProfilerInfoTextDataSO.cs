using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Profiler/InfoText")]
public class ProfilerInfoTextDataSO : ScriptableObject
{
    public int id;
    //public string key;
    public string infomationText;
    public string noticeText;
    public EProfilerCategory category;
    public string infoName;
}
