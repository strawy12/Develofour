using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, ProfilerCategoryDataSO> profilerCategoryDataResourcesList;

    public ProfilerCategoryDataSO GetProfileCategory(string key)
    {
        return profilerCategoryDataResourcesList[key];
    }
    public Dictionary<string, ProfilerCategoryDataSO> GetProfilerCategoryList()
    {
        return profilerCategoryDataResourcesList;
    }
    private async void LoadProfilerCategoryResourcesAssets(Action callBack)
    {
        profilerCategoryDataResourcesList = new Dictionary<string, ProfilerCategoryDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerCategoryData", typeof(ProfilerCategoryDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerCategoryDataSO>(handle.Result[i]).Task;
            await task;

            profilerCategoryDataResourcesList.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
