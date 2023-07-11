using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class OutStarProfileResources : ResourcesComponent
{
    //public List<ResourceSO> GetOutStarCharacterProfileToList()
    //{
    //return resourceDictionary.Select(x => x.Value).ToList();
    //}
    public async override void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("OutStarProfile", typeof(OutStarCharacterDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<OutStarCharacterDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
