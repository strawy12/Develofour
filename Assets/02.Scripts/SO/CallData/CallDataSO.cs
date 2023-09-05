using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECallDataType
{
    None = -1,
    OutGoing,
    InComing,
    Return
}

public class CallDataSO : ResourceSO
{
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

    public string callProfileID; // ĳ���� ID;
     
    public ECallDataType callDataType = ECallDataType.None;
    public float delay;

    public CallScreen callScreen; // �ش� ��ȭ�� ��ũ�� Prefab. 
    public string returnCallID; //���ƿ��� ��ȭ ID

    public List<string> needInfoIDList;
    public List<AdditionFile> additionFileIDList;
}
