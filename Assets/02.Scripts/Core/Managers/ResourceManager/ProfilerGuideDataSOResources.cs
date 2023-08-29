using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, ProfilerGuideDataSO> profilerGuideDataSOList;

    public Dictionary<string, ProfilerGuideDataSO> ProfilerGuideDataSOList => profilerGuideDataSOList;

    public ProfilerGuideDataSO GetProfilerGuideDataSO(string key)
    {
        return profilerGuideDataSOList[key];
    }

    private async void LoadProfilerGuideDataSOAssets(Action callBack)
    {
        profilerGuideDataSOList = new Dictionary<string, ProfilerGuideDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerGuideData", typeof(ProfilerGuideDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerGuideDataSO>(handle.Result[i]).Task;
            await task;

            profilerGuideDataSOList.Add(task.Result.guideName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
