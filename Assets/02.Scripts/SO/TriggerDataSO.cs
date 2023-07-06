using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Trigger")]
public class TriggerDataSO : ScriptableObject
{
    public string triggerID = "";
    public List<string> infoDataIDList;
    public string fileID;
    public List<NeedInfoData> needInfoList;
    public string monoLogType;
    public string completeMonologType = "";
    public float delay;
    public bool isFakeInfo;
}
