using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CallOption
{
    public string decisionName;
    public string outGoingCallID;
}

public class CallProfileDataSO : ResourceSO
{

    // 전화를 받지 않을 경우의 통화 ID
    public string defaultCallID;
    public string notExistCallID;

    public float delay;

    public List<CallOption> outGoingCallOptionList;
    public List<string> inCommingCallIDList;
    public List<string> returnCallIDList;

    public string ID
    {
        get { return id; }
        set
        {
            if (!string.IsNullOrEmpty(id))
                return;

            id = value;
        }
    }
}
