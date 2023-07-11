using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECallDataType
{
    None = -1,
    OutGoing,
    InComing,
}

public class CallDataSO : ResourceSO
{
    public string callProfileID;

    public ECallDataType callDataType = ECallDataType.None;
    public float delay;

    // �����ų ���̵�
    public string monologID;
    public string returnCallID;

    public List<string> needInfoIDList;
    public List<AdditionFile> additionFileIDList;
    
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
