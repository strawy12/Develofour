using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Trigger")]
public class TriggerDataSO : ScriptableObject
{
    public int triggerID = 0;
    public List<int> infoDataIDList;
    public int fileID;
    public List<NeedInfoData> needInfoList;
    public int monoLogType;
    public int completeMonologType = 0;
    public float delay;
    public bool isFakeInfo;

    protected int playMonologType = 0;

}
