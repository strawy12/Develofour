using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, TriggerDataSO> triggerDataList;

    public TriggerDataSO GetTriggerDataSOResources(string key)
    {
        if (string.IsNullOrEmpty(key)) return null;
        return triggerDataList[key];
    }

    private async void LoadTriggerDataSOResourcesAssets(Action callBack)
    {
        triggerDataList = new Dictionary<string, TriggerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("TriggerData", typeof(TriggerDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<TriggerDataSO>(handle.Result[i]).Task;
            await task;

            triggerDataList.Add(task.Result.triggerID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
