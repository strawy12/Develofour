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

    public string callProfileID; // 캐릭터 ID;
     
    public ECallDataType callDataType = ECallDataType.None;
    public float delay;

    public CallScreen callScreen; // 해당 통화의 스크린 Prefab. 
    public string returnCallID; //돌아오는 통화 ID

    public List<string> needInfoIDList;
    public List<AdditionFile> additionFileIDList;
}
