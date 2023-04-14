using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Branch/BranchWriteData", fileName = "PostData_")]
public class BranchPostDataSO : ScriptableObject
{
    [Header("PostLine")]
    public string wirteTitle;
    public string wirteInfo;
    public string wirteDate;
    public EBranchWorkCategory workTitle;
    public Sprite writeImage;

    [Header("Post")]
    public string subTitle;
    public string writerName;
    [TextArea(3, 100)]
    public string mainText;
}
