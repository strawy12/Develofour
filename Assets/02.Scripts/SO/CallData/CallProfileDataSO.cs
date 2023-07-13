using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallOption
{
    public string decisionName;
    public string outGoingCallID;
}

public class CallProfileDataSO : ResourceSO
{
    public string monologID;

    // ��ȭ�� ���� ���� ����� ��ȭ ID
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
