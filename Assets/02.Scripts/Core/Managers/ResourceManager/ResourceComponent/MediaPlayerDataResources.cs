using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class MediaPlayerDataResources : ResourcesComponent
{
    public override async void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("MediaPlayerData", typeof(MediaPlayerDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<MediaPlayerDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
