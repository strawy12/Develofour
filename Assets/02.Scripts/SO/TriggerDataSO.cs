using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Trigger")]
public class TriggerDataSO : ResourceSO
{
    public List<string> infoDataIDList;
    public string fileID;
    public List<NeedInfoData> needInfoList;
    public string monoLogType;
    public string completeMonologType = "";
    public float delay;
    public bool isFakeInfo;
}
