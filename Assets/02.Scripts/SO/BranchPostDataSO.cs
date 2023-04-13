using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Branch/BranchWriteData")]
public class BranchPostDataSO : ScriptableObject
{
    public string wirteTitle;
    public string wirteInfo;
    public string wirteDate;
    public EBranchWorkCategory workTitle;
    public Sprite writeImage;
}
