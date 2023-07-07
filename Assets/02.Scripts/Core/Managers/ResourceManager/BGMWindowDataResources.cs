using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, BGMWindowSO> bgmWindowDataList;

    public BGMWindowSO GetBGMWindowDataResources(string key)
    {
        return bgmWindowDataList[key];
    }

    private async void LoadBackgroundBGMWindowDataResourcesAssets(Action callBack)
    {
        bgmWindowDataList = new Dictionary<string, BGMWindowSO>();

        var handle = Addressables.LoadResourceLocationsAsync("BGMWindowData", typeof(BGMWindowSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<BGMWindowSO>(handle.Result[i]).Task;
            await task;

            bgmWindowDataList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
