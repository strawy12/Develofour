using Coffee.UIEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddReturnCallData(string callID, int delay)
    {
        ReturnCallData data = new ReturnCallData();
        data.id = callID;
        data.SetEndDelayTime(delay);

        saveData.returnCallData.Add(data);
    }

    public void RemoveReturnData(string callID)
    {
        ReturnCallData data = saveData.returnCallData.Find(x => x.id == callID);
        saveData.returnCallData.Remove(data);
    }

    public ReturnCallData GetReturnData(string callID)
    {
        return saveData.returnCallData.Find(x => x.id == callID);
    }
}
