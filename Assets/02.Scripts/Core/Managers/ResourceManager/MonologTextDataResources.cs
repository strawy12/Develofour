using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, MonologTextDataSO> monologTextDataList;
    public Dictionary<string, MonologTextDataSO> MonologTextDataList => monologTextDataList;
    public int MonologDataListCount => monologTextDataList.Count;

    public MonologTextDataSO GetMonologTextData(string textID)
    {
        if (monologTextDataList.ContainsKey(textID))
            return monologTextDataList[textID];

        return null;
    }


    private async void LoadMonologTextDataAssets(Action callBack)
    {
        monologTextDataList = new Dictionary<string, MonologTextDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MonologTextData", typeof(MonologTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {

            var task = Addressables.LoadAssetAsync<MonologTextDataSO>(handle.Result[i]).Task;
            await task;

            if (string.IsNullOrEmpty(task.Result.ID)) continue;
            monologTextDataList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
