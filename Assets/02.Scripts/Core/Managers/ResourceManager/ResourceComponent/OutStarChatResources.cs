using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class OutStarChatResources : ResourcesComponent
{
    public override async void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("OutStarChat", typeof(OutStarChatDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<OutStarChatDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
