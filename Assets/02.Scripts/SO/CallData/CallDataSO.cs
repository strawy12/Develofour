using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECallDataType
{
    None = -1,
    OutGoing,
    InComming,
    Return
}

public abstract class CallDataSO : ScriptableObject
{
    private string id;
    public ECallDataType callDataType = ECallDataType.None;
    public float delay;

    // 실행시킬 아이디
    public string monologID;
    public string returnCallID;

    public List<string> needInfoIDList;
    public List<AddtionFile> additionFileIDList;
    
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
