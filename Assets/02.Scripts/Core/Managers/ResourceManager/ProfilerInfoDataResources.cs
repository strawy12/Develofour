using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, ProfilerInfoDataSO> profilerInfoDataList;

    public ProfilerInfoDataSO GetProfilerInfoData(string key)
    {
        return profilerInfoDataList[key];
    }

    private async void LoadProfilerInfoDataAssets(Action callBack)
    {
        profilerInfoDataList = new Dictionary<string, ProfilerInfoDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerInfoData", typeof(ProfilerInfoDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerInfoDataSO>(handle.Result[i]).Task;
            await task;

            profilerInfoDataList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
