using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ProfilerCategoryDataResources : ResourcesComponent
{
    //public Dictionary<string, ProfilerCategoryDataSO> GetProfilerCategoryList()
    //{
    //    return profilerCategoryDataResourcesList;
    //}

    public async override void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfilerCategoryData", typeof(ProfilerCategoryDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfilerCategoryDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
