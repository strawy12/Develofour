using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, BGMWindowSO> backgroundBGMWindowDataResourcesList;

    public BGMWindowSO GetBGMWindowDataResources(int key)
    {
        return backgroundBGMWindowDataResourcesList[key];
    }

    private async void LoadBackgroundBGMWindowDataResourcesAssets(Action callBack)
    {
        backgroundBGMWindowDataResourcesList = new Dictionary<int, BGMWindowSO>();

        var handle = Addressables.LoadResourceLocationsAsync("BGMWindowData", typeof(BGMWindowSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<BGMWindowSO>(handle.Result[i]).Task;
            await task;

            backgroundBGMWindowDataResourcesList.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
