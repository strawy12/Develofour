using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void SetCurrentTime(int currentTime)
    {
        saveData.CurrentTimeData = currentTime;
    }

    public int GetCurrentTime()
    {
        return saveData.CurrentTimeData;
    }
}
