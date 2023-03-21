using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<EMonologTextDataType, MonologTextDataSO> monologTextDataSOList;

    public MonologTextDataSO GetMonologTextDataSO(EMonologTextDataType textType)
    {
        return monologTextDataSOList[textType];
    }

    private async void LoadMonologTextDataSOAssets(Action callBack)
    {
        monologTextDataSOList = new Dictionary<EMonologTextDataType, MonologTextDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MonologTextData", typeof(MonologTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<MonologTextDataSO>(handle.Result[i]).Task;
            await task;

            monologTextDataSOList.Add(task.Result.TextDataType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
