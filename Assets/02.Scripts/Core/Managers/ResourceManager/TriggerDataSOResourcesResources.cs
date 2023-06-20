using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, TriggerDataSO> triggerDataSOResourcesList;

    public TriggerDataSO GetTriggerDataSOResources(int key)
    {
        if (key == 0) return null;
        return triggerDataSOResourcesList[key];
    }

    private async void LoadTriggerDataSOResourcesAssets(Action callBack)
    {
        triggerDataSOResourcesList = new Dictionary<int, TriggerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("TriggerData", typeof(TriggerDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<TriggerDataSO>(handle.Result[i]).Task;
            await task;

            triggerDataSOResourcesList.Add(task.Result.triggerID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
