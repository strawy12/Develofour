using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="SO/Site/Home/RecordData")]
public class HomeSearchRecordDataSO : ScriptableObject
{
    public EChapterDataType characterDataType;

    [Multiline]
    public List<string> records = new List<string>();
}
