using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class FileLockDataResources : ResourcesComponent
{
    public async override void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("FileLockData", typeof(PinLockDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<PinLockDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
