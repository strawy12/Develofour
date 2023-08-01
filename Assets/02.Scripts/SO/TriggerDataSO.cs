using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Trigger")]
public class TriggerDataSO : ResourceSO
{
    [SerializeField]
    public string MEMO;

    public List<string> infoDataIDList;
    public List<NeedInfoData> needInfoList;
    public string monoLogType;
    public string completeMonologType = "";
    public bool isFakeInfo;
}
