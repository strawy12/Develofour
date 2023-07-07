using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, ProfilerInfoTextDataSO> profilerInfoDataList;

    public ProfilerInfoTextDataSO GetProfilerInfoData(string key)
    {
        return profilerInfoDataList[key];
    }

    private async void LoadProfilerInfoDataAssets(Action callBack)
    {
        profilerInfoDataList = new Dictionary<string, ProfilerInfoTextDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerInfoData", typeof(ProfilerInfoTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerInfoTextDataSO>(handle.Result[i]).Task;
            await task;

            profilerInfoDataList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
