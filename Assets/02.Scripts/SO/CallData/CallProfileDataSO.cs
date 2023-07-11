using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallProfileDataSO : ResourceSO
{
    public string monologID;

    // ��ȭ�� ���� ���� ����� ��ȭ ID
    public string notExistCallID;

    public float delay;

    public List<string> outGoingCallIDList;
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
