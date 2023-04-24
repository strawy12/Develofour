using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriggerList
{
    public static List<InformationTrigger> infoList = new List<InformationTrigger>();
    public static void CheckLinkInfos()
    {
        foreach (var info in infoList)
        {
            info.CheckLinkInfo();
        }
    }
}
