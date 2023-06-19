using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, WindowLockDataSO> fileLockDataList;

    public WindowLockDataSO GetFileLockData(int key)
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
        fileLockDataList = new Dictionary<int, WindowLockDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("FileLockData", typeof(WindowLockDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<WindowLockDataSO>(handle.Result[i]).Task;
            await task;

            fileLockDataList.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
