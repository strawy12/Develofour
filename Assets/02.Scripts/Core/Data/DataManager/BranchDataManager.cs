using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddBranchUnLock(BranchPostDataSO data)
    {
        if (!saveData.branchPostLockData.Contains(data.GetPostKey()))
        {
            saveData.branchPostLockData.Add(data.GetPostKey());
        }
    }

    public bool GetBranchUnLockData(BranchPostDataSO data)
    {
        return saveData.branchPostLockData.Contains(data.GetPostKey());
    }
}
