using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/OutStar/TimeChatData")]
public class OutStarTimeChatDataSO : ResourceSO
{
    public string ID { get => id; }
    public string SetID { set => id = value; }

    public DateTime time;

    public List<string> chatDataIDList;
}
