using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<int, MonologTextDataSO> monologTextDataSOList;
    public Dictionary<int, MonologTextDataSO> MonologTextDataSOList => monologTextDataSOList;
    public int MonologDataListCount => monologTextDataSOList.Count;

    public MonologTextDataSO GetMonologTextData(int textType)
    {
        return monologTextDataSOList[textType];
    }


    private async void LoadMonologTextDataAssets(Action callBack)
    {
        monologTextDataSOList = new Dictionary<int, MonologTextDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MonologTextData", typeof(MonologTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {

            var task = Addressables.LoadAssetAsync<MonologTextDataSO>(handle.Result[i]).Task;
            await task;
            if(task.Result.TextDataType == 43)
            Debug.Log(task.Result.name);

            if (task.Result.TextDataType == 0) continue;
            monologTextDataSOList.Add(task.Result.TextDataType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
