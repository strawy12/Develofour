using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddCallSave(string callID)
    {
        if (saveData.callSaveData.Contains(callID)) return;
        saveData.callSaveData.Add(callID);
    }

    public bool IsSaveCallData(string callID)
    {
        return saveData.callSaveData.Contains(callID);
    }
}
