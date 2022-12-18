using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWorkTitle
{
    None,
}
[CreateAssetMenu(menuName = "SO/Site/Brunch/Work")]
public class BrunchWorkDataSO : ScriptableObject
{
    public Sprite workSprite;
    public EWorkTitle titleName;
    public int writeCnt;
    public int userCnt;
}
