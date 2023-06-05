using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AddressableAssets.HostingServices;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "SO/Window/Mail/Data")]
public class MailDataSO : ScriptableObject
{
    public int mailID;

    public string receiveName;
    public string sendName;
    public string titleText;
    public string informationText;

    public Sprite userProfile;

    public Vector3Int dateData; //x: year, y: month, z: date
    public Vector2Int timeData; // x: hour, y: minute

    [BitMask(typeof(EEmailCategory))]
    public int mailCategory;

    public int Year => dateData.x;
    public int Month => dateData.y;
    public int Date => dateData.z;

    public int Hour => timeData.x;
    public int Minute => timeData.y;

    public string TimeText => $"{Year}. {Month}. {Date}. {Hour}:{Minute}";

    public bool isFavorited { get { return mailCategory.ContainMask((int)EEmailCategory.Favorite); } }

    public long GetCompareFlagValue()
    {
        string str = TimeText;
        return long.Parse(str);
    }
}
