using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<EProfilerCategory, ProfilerCategoryDataSO> profilerCategoryDataResourcesList;

    public ProfilerCategoryDataSO GetProfileCategory(EProfilerCategory key)
    {
        return profilerCategoryDataResourcesList[key];
    }
    public Dictionary<EProfilerCategory, ProfilerCategoryDataSO> GetProfilerCategoryList()
    {
        return profilerCategoryDataResourcesList;
    }
    private async void LoadProfilerCategoryResourcesAssets(Action callBack)
    {
        profilerCategoryDataResourcesList = new Dictionary<EProfilerCategory, ProfilerCategoryDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerCategoryData", typeof(ProfilerCategoryDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerCategoryDataSO>(handle.Result[i]).Task;
            await task;

            profilerCategoryDataResourcesList.Add(task.Result.category, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
