using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, ProfilerGuideDataSO> profilerGuideDataSOResourcesList;

    public Dictionary<string, ProfilerGuideDataSO> ProfilerGuideDataSOResourcesList
    {
        get => profilerGuideDataSOResourcesList;
    }

    public ProfilerGuideDataSO GetProfilerGuideDataSOResources(string key)
    {
        return profilerGuideDataSOResourcesList[key];
    }

    private async void LoadProfilerGuideDataSOResourcesAssets(Action callBack)
    {
        profilerGuideDataSOResourcesList = new Dictionary<string, ProfilerGuideDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerGuideData", typeof(ProfilerGuideDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerGuideDataSO>(handle.Result[i]).Task;
            await task;

            profilerGuideDataSOResourcesList.Add(task.Result.guideName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
