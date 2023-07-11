using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class MonologTextDataResources : ResourcesComponent
{
    public int MonologDataListCount => resourceDictionary.Count;

    public override async void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MonologTextData", typeof(MonologTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {

            var task = Addressables.LoadAssetAsync<MonologTextDataSO>(handle.Result[i]).Task;
            await task;

            if (string.IsNullOrEmpty(task.Result.ID)) continue;
            resourceDictionary.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
