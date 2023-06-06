using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBranchWorkCategory
{
    None,
    MyDiary,
    HardHealth,
    Date,
    MyStory,
}

[CreateAssetMenu(menuName = "SO/Site/Branch/Work")]
public class BranchWorkDataSO : ScriptableObject
{
    public Sprite workSprite;
    public EBranchWorkCategory workKey;
    public string titleText;
    public int userCnt;

    public List<BranchPostDataSO> postDataList;
}
