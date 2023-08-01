using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/OutStar/ChatData")]
public class OutStarChatDataSO : ResourceSO
{
    public string ID { get => id; }
    public string SetID { set => id = value; }

    public bool isMine;
    public string chatText;
    public List<OutStarTrigger> outStarTriggerList;
}

[System.Serializable]
public class OutStarTrigger
{
    public int startIdx;
    public int endIdx;
    public string triggerID;

    public OutStarTrigger(string id, int startIdx, int endIdx)
    {
        this.startIdx = startIdx;
        this.endIdx = endIdx;
        triggerID = id;
    }
}
