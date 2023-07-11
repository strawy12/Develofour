using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Profiler/InfoText")]
public class ProfilerInfoDataSO : ResourceSO
{
    //public string key;
    public string infomationText;
    public string noticeText;
    public string categoryID;
    public string infoName;

    public string ID
    {
        get => id;
        set
        {
            if (string.IsNullOrEmpty(id))
            {
                id = value;
            }
        }
    }
}
