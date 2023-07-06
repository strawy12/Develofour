using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, CallDataSO> callDataList;

    public CallDataSO GetCallData(string key)
    {
        if(callDataList.ContainsKey(key))
        {
            return callDataList[key];
        }
        return null;
    }
    private async void LoadCallDataResourcesAssets(Action callBack)
    {
        callDataList = new Dictionary<string, CallDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("CallData", typeof(CallDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CallDataSO>(handle.Result[i]).Task;
            await task;

            callDataList.Add(task.Result.ID, task.Result); 
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
