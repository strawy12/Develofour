using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Brunch/BrunchWriteData")]
public class BrunchPostDataSO : ScriptableObject
{
    public string wirteTitle;
    public string wirteInfo;
    public string wirteDate;
    public EWorkTitle workTitle;
    public Sprite writeImage;
}
