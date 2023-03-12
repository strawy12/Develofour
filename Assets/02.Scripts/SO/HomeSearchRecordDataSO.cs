using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="SO/Site/Home/RecordData")]
public class HomeSearchRecordDataSO : ScriptableObject
{
    [Multiline]
    public List<string> records = new List<string>();
}
