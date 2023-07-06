using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, PinLockDataSO> fileLockDataList;

    public PinLockDataSO GetPinLockData(string key)
    {
        if(fileLockDataList == null)
        {
            return null;
        }
        if (!fileLockDataList.ContainsKey(key))
        {
            return null;
        }

        return fileLockDataList[key];
    }

    private async void LoadFileLockDataAssets(Action callBack)
    {
        fileLockDataList = new Dictionary<string, PinLockDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("FileLockData", typeof(PinLockDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<PinLockDataSO>(handle.Result[i]).Task;
            await task;

            fileLockDataList.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
