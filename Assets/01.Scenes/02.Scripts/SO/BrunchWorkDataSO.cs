using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWorkKeys
{
    None,
    AIDoctor,
    Eyewitnesses,
}
[CreateAssetMenu(menuName = "SO/Site/Brunch/Work")]
public class BrunchWorkDataSO : ScriptableObject
{
    public Sprite workSprite;
    public EWorkKeys workKey;
    public string titleText;
    public int writeCnt;
    public int userCnt;
}
